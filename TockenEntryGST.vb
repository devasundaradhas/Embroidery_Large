Public Class TockenEntryGST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSALE-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private vcmb_ItmNm As String
    Private vcmb_SizNm As String
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetMxIndx As Integer

    Private DetIndx As Integer
    Private DetSNo As Integer
    Private Print_PDF_Status As Boolean = False
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_DetDt1 As New DataTable
    Private prn_DetIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_DetAr(100, 50, 10) As String
    Private prn_OriDupTri As String = ""
 

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub clear()
        Dim obj As Object
        Dim ctrl1 As Object, ctrl2 As Object, ctrl3 As Object
        Dim pnl1 As Panel, pnl2 As Panel
        Dim grpbx As Panel

        New_Entry = False
        Insert_Entry = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False
        pnl_Selection.Visible = False
      
        'dtp_InTime.Text = Format(Now, "dd/MM/yyyy hh:mm tt")

        SetMyCustomFormat()


        cbo_VehicleNo.Text = ""
        cbo_Vehicle_type.Text = ""
        cbo_Ledger.Text = ""
        txt_Address1.Text = ""
        txt_Address2.Text = ""
        txt_Address3.Text = ""
        txt_Address4.Text = ""
        Cbo_MobileNo.Text = ""
        lbl_Total_Hrs.Text = ""
        lbl_TotalDays.Text = ""

        msk_inTime.Text = dtp_InTime.Text
        msk_outTime.Text = "" 'dtp_OutTime.Text

        rBtn_Daily.Checked = True

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""

            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""

            ElseIf TypeOf obj Is DateTimePicker Then
                obj.text = ""

            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl1 In grpbx.Controls
                    If TypeOf ctrl1 Is TextBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is ComboBox Then
                        ctrl1.text = ""
                    ElseIf TypeOf ctrl1 Is DateTimePicker Then
                        ctrl1.text = ""
                    End If
                Next

            ElseIf TypeOf obj Is Panel Then
                pnl1 = obj
                If Trim(UCase(pnl1.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                    For Each ctrl2 In pnl1.Controls
                        If TypeOf ctrl2 Is TextBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is ComboBox Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is DataGridView Then
                            ctrl2.Rows.Clear()
                        ElseIf TypeOf ctrl2 Is DateTimePicker Then
                            ctrl2.text = ""
                        ElseIf TypeOf ctrl2 Is Panel Then
                            pnl2 = ctrl2
                            If Trim(UCase(pnl2.Name)) <> Trim(UCase(pnl_Filter.Name)) Then
                                For Each ctrl3 In pnl2.Controls
                                    If TypeOf ctrl3 Is TextBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is ComboBox Then
                                        ctrl3.text = ""
                                    ElseIf TypeOf ctrl3 Is DateTimePicker Then
                                        ctrl3.text = ""
                                    End If
                                Next
                            End If

                        End If

                    Next

                End If

            End If

        Next


     


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Grid_Cell_DeSelect()
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Grid_Cell_DeSelect()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
        If FrmLdSTS = True Then Exit Sub
     
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        NoCalc_Status = True

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as LedgerName, B.* from Tocken_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Tocken_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Tocken_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Tocken_Date").ToString


                If IsDBNull(dt1.Rows(0).Item("LedgerName").ToString) = False Then
                    cbo_Ledger.Text = Trim(dt1.Rows(0).Item("LedgerName").ToString)
                End If

                If Val(dt1.Rows(0).Item("Ledger_Idno").ToString) = 0 Then
                    cbo_Ledger.Text = Trim(dt1.Rows(0).Item("Party_Name").ToString)
                    txt_Address1.Text = Trim(dt1.Rows(0).Item("Address1").ToString)
                    txt_Address2.Text = Trim(dt1.Rows(0).Item("Address2").ToString)
                    txt_Address3.Text = Trim(dt1.Rows(0).Item("Address3").ToString)
                    txt_Address4.Text = Trim(dt1.Rows(0).Item("Address4").ToString)
                    Cbo_MobileNo.Text = Trim(dt1.Rows(0).Item("Mobile_No").ToString)

                Else
                    txt_Address1.Text = Trim(dt1.Rows(0).Item("Ledger_Address1").ToString)
                    txt_Address2.Text = Trim(dt1.Rows(0).Item("Ledger_Address2").ToString)
                    txt_Address3.Text = Trim(dt1.Rows(0).Item("Ledger_Address3").ToString)
                    txt_Address4.Text = Trim(dt1.Rows(0).Item("Ledger_Address4").ToString)
                    Cbo_MobileNo.Text = Trim(dt1.Rows(0).Item("Ledger_PhoneNo").ToString)

                End If
                
                cbo_VehicleNo.Text = Trim(dt1.Rows(0).Item("Vehicle_No").ToString)
                cbo_Vehicle_type.Text = Trim(dt1.Rows(0).Item("Vehicle_Type").ToString)

                msk_inTime.Text = Trim(dt1.Rows(0).Item("InTime").ToString)
                dtp_InTime.Text = Trim(dt1.Rows(0).Item("InTime").ToString)

                If InStr(Trim(dt1.Rows(0).Item("OutTime").ToString), "1990") = 0 Then
                    msk_outTime.Text = Trim(dt1.Rows(0).Item("OutTime").ToString)
                    dtp_OutTime.Text = Trim(dt1.Rows(0).Item("OutTime").ToString)
                End If


                dtp_InDateTime.Text = Trim(dt1.Rows(0).Item("InDateTime").ToString)

                If InStr(Trim(dt1.Rows(0).Item("OutDateTime").ToString), "1990") = 0 Then

                    dtp_OutDateTime.Text = Trim(dt1.Rows(0).Item("OutDateTime").ToString)

                End If


                lbl_Total_Hrs.Text = Format(Val(dt1.Rows(0).Item("Total_Hrs").ToString), "###########")
                lbl_TotalDays.Text = Format(Val(dt1.Rows(0).Item("Total_Days").ToString), "###########")

                lbl_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "###########")
                txt_amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "###########")
                cbo_Vehicle_type.Text = Trim(dt1.Rows(0).Item("Vehicle_Type").ToString)

                If Trim(dt1.Rows(0).Item("Tocken_Type").ToString) = "MONTH" Then
                    rBtn_Monthly.Checked = True
                ElseIf Trim(dt1.Rows(0).Item("Tocken_Type").ToString) = "DAY" Then
                    rBtn_Daily.Checked = True
                ElseIf Trim(dt1.Rows(0).Item("Tocken_Type").ToString) = "HOUR" Then
                    rBtn_Hr.Checked = True
                End If



                NoCalc_Status = False

            End If

            dt1.Clear()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da2.Dispose()

            dt1.Dispose()
            da1.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub SalesEntry_Simple1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

          

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub SalesEntry_Simple1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        'If Trim(UCase(Common_Procedures.SalesEntryType)) = "LABOUR INVOICE" Then

        '    If Trim(UCase(Common_Procedures.Sales_Or_Service)) = "SALES" Then
        '        Pk_Condition = "GLBIN-"
        '        lbl_Title.Text = "LABOUR INVOICE"
        '    Else
        '        Pk_Condition = "GLBSR-"
        '        lbl_Title.Text = "LABOUR INVOICE"
        '    End If
        'Else
        '    Pk_Condition = "GSALE-"
        '    lbl_Title.Text = "TAX INVOICE"
        '   End If



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

    

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()





        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vehicle_type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_inTime.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_outTime.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_MobileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_amount.GotFocus, AddressOf ControlGotFocus

    

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vehicle_type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_inTime.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_outTime.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_MobileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_amount.LostFocus, AddressOf ControlLostFocus

      


        ' AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
      
        ' AddHandler txt_CashDiscPerc.KeyDown, AddressOf TextBoxControlKeyDown
       

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address4.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_inTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_outTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_InTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_OutTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_InDateTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_OutDateTime.KeyPress, AddressOf TextBoxControlKeyPress
      
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_amount.KeyPress, AddressOf TextBoxControlKeyPress

       
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address4.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_inTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_outTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_InTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_OutTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_InDateTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_OutDateTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_amount.KeyDown, AddressOf TextBoxControlKeyDown

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub SalesEntry_Simple1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub SalesEntry_Simple1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

            


                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Close_Form()

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
            lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
            Me.Text = lbl_Company.Text
            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con


            cmd.CommandText = "delete from Tocken_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
         


            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select Tocken_No from Tocken_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code  like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, Tocken_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString

                    End If
                End If
            End If

            dr.Close()

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select Tocken_No from Tocken_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, Tocken_No", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select Tocken_No from Tocken_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Tocken_No desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If
            dr.Close()

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select Tocken_No from Tocken_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Tocken_No desc", con)
        Dim dt As New DataTable
        Dim movno As String

        Try
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If movno <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Tocken_Head", "Tocken_Code", "For_OrderBy", "Tocken_Code LIKE '" & Trim(Pk_Condition) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select  a.* from Tocken_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Tocken_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Tocken_No desc", con)
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
              
                '  If dt2.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt2.Rows(0).Item("Tax_Type").ToString
                ' If dt2.Rows(0).Item("Tax_Perc").ToString <> "" Then txt_VatPerc.Text = Val(dt2.Rows(0).Item("Tax_Perc").ToString)

            End If

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Tocken No.", "FOR FINDING...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Tocken_No from Tocken_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Tocken No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

     
        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
       
        Try

            inpno = InputBox("Enter New Invoice No.", "FOR INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Sales_No from Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Invoice No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim saleac_id As Integer = 0
        Dim txac_id As Integer = 0
        Dim itm_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim Sz_id As Integer = 0
        Dim Sno As Integer = 0
        Dim Ac_id As Integer = 0
        Dim vTot_Qty As Single = 0
        Dim itm_GrpId As Integer = 0
        Dim VouType As String = ""
        Dim CsParNm As String
        Dim dttm As DateTime
        Dim Temp_dttm As DateTime = "01-01-1990"
        Dim Tkn_Type As String = ""
        Dim SurName As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
      
        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sales_Entry, New_Entry) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Trim(cbo_VehicleNo.Text) = "" Then
            MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_VehicleNo.Enabled Then cbo_VehicleNo.Focus()
            Exit Sub
        End If

        If dtp_Date.Value.Date <> Convert.ToDateTime(msk_inTime.Text) Then
            dtp_InTime.Value = dtp_Date.Value
            msk_inTime.Text = dtp_Date.Text
        End If


        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)



        CsParNm = ""
        If led_id = 1 And Trim(CsParNm) = "" Then
            CsParNm = "Cash"
        End If



        If rBtn_Hr.Checked = True Then
            Tkn_Type = "HOUR"
        ElseIf rBtn_Daily.Checked = True Then
            Tkn_Type = "DAY"
        ElseIf rBtn_Monthly.Checked = True Then
            Tkn_Type = "MONTH"
        End If


        SurName = Common_Procedures.Remove_NonCharacters(Trim(cbo_VehicleNo.Text))


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Tocken_Head", "Tocken_Code", "For_OrderBy", "Tocken_Code LIKE '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@TockenDate", dtp_Date.Value.Date)




            If Trim(msk_inTime.Text) <> "/  /" Then
                cmd.Parameters.AddWithValue("@InDate", dtp_InTime.Value.Date)
                dttm = Convert.ToDateTime(msk_inTime.Text & " " & dtp_InDateTime.Text)
                cmd.Parameters.AddWithValue("@InDateTime", dttm)
            Else
                cmd.Parameters.AddWithValue("@InDate", Temp_dttm)
                cmd.Parameters.AddWithValue("@InDateTime", Temp_dttm)
            End If




            If Trim(msk_outTime.Text) <> "/  /" Then
                cmd.Parameters.AddWithValue("@OutDate", dtp_OutTime.Value.Date)
                dttm = Convert.ToDateTime(msk_outTime.Text & " " & dtp_OutDateTime.Text)
                cmd.Parameters.AddWithValue("@OutDateTime", dttm)
            Else
                cmd.Parameters.AddWithValue("@OutDate", Temp_dttm)
                cmd.Parameters.AddWithValue("@OutDateTime", Temp_dttm)
            End If


            If Trim(txt_amount.Text) = "" Then txt_amount.Text = 0

            If New_Entry = True Then

                cmd.CommandText = "Insert into Tocken_Head(Tocken_Code ,             Company_IdNo         ,              Tocken_No               ,                               for_OrderBy                               , Tocken_Date    ,           Vehicle_No             , Sur_Name                ,       Ledger_Idno       ,       InTime   ,        OutTime   ,InDateTime   ,OutDateTime   ,Total_Hrs                       ,Total_Days                     ,Party_Name                    , Address1                         ,Address2                         ,Address3                         ,Address4                         ,Mobile_No                        ,Rate                       ,Amount                       ,Tocken_Type             ,Vehicle_Type) " & _
                                    " Values ('" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @TockenDate    ,'" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(SurName) & "' ," & Str(Val(led_id)) & ",      @InDate  ,        @OutDate  , @InDateTime ,@OutDateTime  , " & Val(lbl_Total_Hrs.Text) & "," & Val(lbl_TotalDays.Text) & ",'" & Trim(cbo_Ledger.Text) & "','" & Trim(txt_Address1.Text) & "' ,'" & Trim(txt_Address2.Text) & "','" & Trim(txt_Address3.Text) & "','" & Trim(txt_Address4.Text) & "','" & Trim(Cbo_MobileNo.Text) & "'," & Val(lbl_Rate.Text) & " ," & Val(txt_amount.Text) & ",'" & Trim(Tkn_Type) & "','" & Trim(cbo_Vehicle_type.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Tocken_Head set Tocken_Date = @TockenDate, InTime = @InDate ,OutTime =@OutDate , InDateTime =@InDateTime , OutDateTime =  @OutDateTime , Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' ,Sur_Name = '" & Trim(SurName) & "' , Ledger_Idno =  " & Str(Val(led_id)) & " ,Total_Hrs = " & Val(lbl_Total_Hrs.Text) & ",Total_Days = " & Val(lbl_TotalDays.Text) & ", Party_Name ='" & Trim(cbo_Ledger.Text) & "' , Address1  ='" & Trim(txt_Address1.Text) & "' ,Address2  ='" & Trim(txt_Address2.Text) & "' ,Address3  ='" & Trim(txt_Address3.Text) & "' ,Address4 ='" & Trim(txt_Address4.Text) & "' ,Mobile_No  ='" & Trim(Cbo_MobileNo.Text) & "',Rate = " & Val(lbl_Rate.Text) & " ,Amount = " & Val(txt_amount.Text) & " ,Tocken_Type = '" & Trim(Tkn_Type) & "',Vehicle_Type = '" & Trim(cbo_Vehicle_type.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            VouType = "Gst.Bill"

            Dim vVouPos_IdNos As String = "", vVouPos_Amts As String = "", vVouPos_ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0


            AcPos_ID = 1   'CASH
            saleac_id = 28 ' SERVICE REVENUE

            Dim vNetAmt As String = Format(Val(CSng(txt_amount.Text)), "#############0.00")

            '---GST
            vVouPos_IdNos = AcPos_ID & "|" & saleac_id

            vVouPos_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt)

            If Common_Procedures.Voucher_Updation(con, Trim(VouType), Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Token No . : " & Trim(lbl_InvoiceNo.Text), vVouPos_IdNos, vVouPos_Amts, vVouPos_ErrMsg, tr) = False Then
                Throw New ApplicationException(vVouPos_ErrMsg)
            End If



            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Err.Description, "CK_Sales_Delivery_Details_1") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Sales_Delivery_Details_2") > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Invoice Quantity must be lesser than Delivery Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            tr.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try



    End Sub


    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        '  If e.KeyCode = 38 Then e.Handled = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub





    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10   )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Vehicle_type, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        vcbo_KeyDwnVal = e.KeyValue
        ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_DcDate, txt_Electronic_RefNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        '  Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Electronic_RefNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub



    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = " a.Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sales_Code IN (select z.Sales_Code from Sales_Details z where z.Item_IdNo = " & Str(Val(Itm_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.Sales_No, a.Sales_Date, a.Total_Qty, a.Net_Amount, b.Ledger_Name from Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            dt1.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True 'SendKeys.Send("+{TAB}")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, cbo_Filter_PartyName, btn_Filter_Show, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub








    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub





    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub



    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub





    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Tocken_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Tocken_Code = '" & Trim(NewCode) & "' ", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If


            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'prn_InpOpts = ""
        'prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "12")

        'prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        If Trim(Common_Procedures.settings.CustomerCode) = "1193" Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3X30", 300, 3000)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        Else

        End If


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        'e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If
                End If


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
        Print_PDF_Status = False
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_DetDt1.Clear()
        prn_Count = 0
        DetIndx = 0

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Tocken_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON b.State_Idno = Lsh.State_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON c.Company_State_IdNo = csh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Tocken_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count = 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format5(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNmDesc As String = ""
        Dim ItmDescAr(20) As String

        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim m1 As Integer = 0
        Dim k As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 35
            .Bottom = 35
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 19
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
            If Len(prn_HdDt.Rows(0).Item("Company_Description").ToString) > 75 Then NoofItems_PerPage = NoofItems_PerPage - 1
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 350 : ClArr(3) = 85 : ClArr(4) = 80 : ClArr(5) = 85
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNmDesc = Common_Procedures.Item_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString))
                        If (prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString) <> "" Then
                            ItmNmDesc = Trim(ItmNmDesc) & "  -  " & prn_DetDt.Rows(prn_DetIndx).Item("Item_Description").ToString
                        End If

                        Erase ItmDescAr
                        ItmDescAr = New String(20) {}

                        m1 = -1

LOOP1:
                        If Len(ItmNmDesc) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(ItmNmDesc, k, 1) = " " Or Mid$(ItmNmDesc, k, 1) = "," Or Mid$(ItmNmDesc, k, 1) = "/" Or Mid$(ItmNmDesc, k, 1) = "\" Or Mid$(ItmNmDesc, k, 1) = "-" Or Mid$(ItmNmDesc, k, 1) = "." Or Mid$(ItmNmDesc, k, 1) = "&" Or Mid$(ItmNmDesc, k, 1) = "_" Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            m1 = m1 + 1
                            ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), k)
                            'ItmDescAr(m1) = Microsoft.VisualBasic.Left(Trim(ItmNmDesc), K - 1)
                            ItmNmDesc = Microsoft.VisualBasic.Right(ItmNmDesc, Len(ItmNmDesc) - k)
                            GoTo LOOP1

                        Else

                            m1 = m1 + 1
                            ItmDescAr(m1) = ItmNmDesc

                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(0)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Noof_Items").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Unit_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Unit_IdNO").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1


                        For k = 1 To m1
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDescAr(k)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        Next k

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim i As Integer = 0
        Dim strHeight As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim C1 As Single, W1 As Single, S1 As Single, S2 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_ShrtName As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim strWidth As String
        Dim CurX As Single = 0
        Dim OrdNoDt As String = ""
        Dim DcNoDt As String = ""


        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sales_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Code = '" & Trim(EntryCode) & "'  Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Cmp_ShrtName = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_ShrtName = prn_HdDt.Rows(0).Item("Company_ShortName").ToString

        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Then '---- Sri Arul Engineering Works
            If InStr(1, Trim(UCase(Cmp_Name)), "ARUL") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Company_Logo_Arul, Drawing.Image), LMargin, CurY + 15, 130, 100)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "AVS") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "A V S") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "A.V.S") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Billing.My.Resources.Resources.Company_Logo_Avs, Drawing.Image), LMargin, CurY + 15, 130, 100)
            End If
        End If

        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString)
        ItmNm2 = ""
        If Trim(ItmNm1) <> "" Then
            ItmNm1 = "(" & Trim(ItmNm1) & ")"
            If Len(ItmNm1) > 85 Then
                For i = 85 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 85
                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
            End If
        End If

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If

        CurY = CurY + strHeight - 0.5
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)


        strWidth = e.Graphics.MeasureString(Cmp_PhNo & "      " & Cmp_Email, pFont).Width

        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, CurX, CurY, 0, PrintWidth, pFont)


        strWidth = e.Graphics.MeasureString(Cmp_PhNo, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "      " & Cmp_Email, CurX, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1091" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1154" Then '---- Sri Arul Engineering Works (Thekkalur)
            If Trim(UCase(Pk_Condition)) = "GLBIN-" Then
                Common_Procedures.Print_To_PrintDocument(e, "LABOUR BILL", LMargin, CurY, 2, PrintWidth, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "CASH BILL", LMargin, CurY, 2, PrintWidth, p1Font)
            End If
        Else
            If Trim(UCase(Pk_Condition)) = "GLBIN-" Then
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            End If
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            S2 = e.Graphics.MeasureString("ORDER.NO & DATE :    ", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            OrdNoDt = prn_HdDt.Rows(0).Item("Order_No").ToString
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    OrdNoDt = Trim(OrdNoDt) & "  Dt : " & Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString)
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(OrdNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(OrdNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If


            DcNoDt = prn_HdDt.Rows(0).Item("Dc_No").ToString
            'If Trim(prn_HdDt.Rows(0).Item("Dc_date").ToString) <> "" Then
            '    DcNoDt = Trim(DcNoDt) & "  Dt : " & Trim(prn_HdDt.Rows(0).Item("Dc_date").ToString)
            'End If

            CurY = CurY + TxtHgt
            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(DcNoDt) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(DcNoDt), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt - 5
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim p1Font As Font
        Dim I As Integer
        Dim BmsInWrds As String
        Dim W1 As Single = 0
        Dim CurY1 As Single = 0
        Dim Cmp_Name As String
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim Yax As Single
        Dim vprn_PckNos As String = ""
        Dim Tot_Wgt As Single = 0, Tot_Amt As Single = 0, Tot_Bgs As Single = 0, Tot_Wgt_Bag As Single = 0
        W1 = e.Graphics.MeasureString("Payment Terms : ", pFont).Width

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Qty").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, " " & (prn_HdDt.Rows(0).Item("gROSS_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If

            CurY = CurY - 10


            CurY = CurY + TxtHgt + 1
            If Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Cash Discount @ " & Trim(Val(prn_HdDt.Rows(0).Item("CashDiscount_Perc").ToString)) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CashDiscount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Perc").ToString)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then

                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Freight Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 3
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then

                If is_LastPage = True Then

                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Add Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                End If
            End If

            CurY = CurY + TxtHgt + 3

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            CurY = CurY + TxtHgt - 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            StrConv(BmsInWrds, vbProperCase)

            Common_Procedures.Print_To_PrintDocument(e, "Rupees    : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            NoofHsnCodes = Dt1.Rows.Count
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()

        get_GST_Noof_HSN_Codes_For_Printing = NoofHsnCodes

    End Function


    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Sales_GST_Tax_Details Where Sales_Code = '" & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                        TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
                    End If
                End If
                Dt2.Clear()

            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function
    Public Sub SetMyCustomFormat()

        dtp_InTime.Format = DateTimePickerFormat.Custom
        dtp_InTime.CustomFormat = "dd/MM/yyyy"

        dtp_OutTime.Format = DateTimePickerFormat.Custom
        dtp_OutTime.CustomFormat = "dd/MM/yyyy"

    End Sub

    Private Sub msk_inTime_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_inTime.GotFocus
        lbl_DateToolTip.Visible = True
    End Sub

    Private Sub msk_inTime_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_inTime.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_inTime.Text = Date.Today
        'End If
        'If e.KeyCode = 107 Then
        '    msk_inTime.Text = DateAdd("D", 1, Convert.ToDateTime(msk_inTime.Text))
        'ElseIf e.KeyCode = 109 Then
        '    msk_inTime.Text = DateAdd("D", -1, Convert.ToDateTime(msk_inTime.Text))
        'End If
        'If UCase(Chr(e.KeyCode)) = "D" Then
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            msk_inTime.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

        End If
    End Sub

    Private Sub msk_inTime_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_inTime.LostFocus

        If IsDate(msk_inTime.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_inTime.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_inTime.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_inTime.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_inTime.Text)) >= 2000 Then
                    dtp_InTime.Value = Convert.ToDateTime(msk_inTime.Text)
                End If
            End If
        End If
        lbl_DateToolTip.Visible = False
    End Sub

    Private Sub msk_outTime_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_outTime.GotFocus
        lbl_DateToolTip.Visible = True
    End Sub
    Private Sub msk_outTime_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_outTime.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_inTime.Text = Date.Today
        'End If
        'If e.KeyCode = 107 Then
        '    msk_inTime.Text = DateAdd("D", 1, Convert.ToDateTime(msk_inTime.Text))
        'ElseIf e.KeyCode = 109 Then
        '    msk_inTime.Text = DateAdd("D", -1, Convert.ToDateTime(msk_inTime.Text))
        'End If

        '
        '  If UCase(Chr(e.KeyCode)) = "D" Then
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            msk_outTime.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

        End If
    End Sub

    Private Sub msk_outTime_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_outTime.LostFocus

        lbl_DateToolTip.Visible = False

        If IsDate(msk_outTime.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_outTime.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_outTime.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_outTime.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_outTime.Text)) >= 2000 Then
                    dtp_OutTime.Value = Convert.ToDateTime(msk_outTime.Text)
                End If
            End If
        End If

    End Sub

    Private Sub dtp_InTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_InTime.TextChanged

        If IsDate(dtp_Date.Text) = True Then
            SetMyCustomFormat()
            msk_inTime.Text = dtp_InTime.Text
            msk_inTime.SelectionStart = 0
        End If
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus


        If rBtn_Monthly.Checked = True Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Token_Monthly_Head", "Vehicle_No", "", "")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Tocken_Head", "Vehicle_No", "", "")
        End If


    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, dtp_Date, cbo_Vehicle_type, "Token_Monthly_Head", "Vehicle_No", "", "")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, dtp_Date, cbo_Vehicle_type, "Tocken_Head", "Vehicle_No", "", "")
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Try
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Vehicle_type, "Token_Monthly_Head", "Vehicle_No", "", "", False)
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Vehicle_type, "Tocken_Head", "Vehicle_No", "", "", False)
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_Vehicle_type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vehicle_type.GotFocus
        If rBtn_Monthly.Checked = True Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Token_Monthly_Head", "Vehicle_Type", "", "")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Tocken_Head", "Vehicle_Type", "", "")
        End If

    End Sub

    Private Sub cbo_Vehicle_type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vehicle_type.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vehicle_type, cbo_VehicleNo, Cbo_MobileNo, "Token_Monthly_Head", "Vehicle_Type", "", "")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vehicle_type, cbo_VehicleNo, Cbo_MobileNo, "Tocken_Head", "Vehicle_Type", "", "")
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub cbo_Vehicle_type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vehicle_type.KeyPress
        Try
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vehicle_type, Cbo_MobileNo, "Token_Monthly_Head", "Vehicle_Type", "", "", False)
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vehicle_type, Cbo_MobileNo, "Tocken_Head", "Vehicle_Type", "", "", False)
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Get_Hours()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim SurName As String = ""
        Dim Dttm1 As DateTime
        Dim Dttm2 As DateTime
        Dim Temp_Dttm As DateTime = "01-01-1990"
        Dim StartDate As DateTime
        Dim EndDate As DateTime
        Dim CurrentDate As DateTime
        Dim Hrs As Single = 0
        Dim Days As Single = 0

        If NoCalc_Status = True Then Exit Sub


        If Trim(msk_inTime.Text) <> "/  /" And Trim(msk_inTime.Text) <> "" Then
            If IsDate(msk_inTime.Text) Then
                Dttm1 = Convert.ToDateTime(msk_inTime.Text & " " & dtp_InDateTime.Text)
            End If

        Else
            Dttm2 = Temp_Dttm
        End If

        If Trim(msk_outTime.Text) <> "/  /" And Trim(msk_outTime.Text) <> "" Then
            If IsDate(msk_outTime.Text) Then
                Dttm2 = Convert.ToDateTime(msk_outTime.Text & " " & dtp_OutDateTime.Text)

            End If

        Else
            Dttm2 = Temp_Dttm
        End If

        If Dttm1 <> Temp_Dttm And Dttm2 <> Temp_Dttm And IsDate(msk_outTime.Text) And IsDate(msk_outTime.Text) Then

            Hrs = DateDiff(DateInterval.Hour, Dttm1, Dttm2)
            Days = DateDiff(DateInterval.Day, Dttm1, Dttm2)

        Else
            Hrs = 0
            Days = 0
        End If


        lbl_Total_Hrs.Text = IIf(Hrs > 0, Format(Val(Hrs), "#########"), 0)

        If rBtn_Monthly.Checked = True Then

            SurName = Common_Procedures.Remove_NonCharacters(Trim(cbo_VehicleNo.Text))
            da1 = New SqlClient.SqlDataAdapter("select * from Token_Monthly_Head where Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Sur_Name = '" & Trim(SurName) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                If Not IsDBNull(dt1.Rows(0).Item("Close_Status").ToString) Then
                    If Val(dt1.Rows(0).Item("Close_Status").ToString) = 0 Then
                        StartDate = dt1.Rows(0).Item("StartDate")
                        EndDate = dt1.Rows(0).Item("EndDate")

                        CurrentDate = dtp_Date.Text

                        lbl_TotalDays.Text = Val(DateDiff(DateInterval.Day, StartDate, CurrentDate)) + 1



                    End If
                End If
            End If

            dt1.Clear()
            da1.Dispose()
        End If



       



    End Sub

    Private Sub msk_inTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_inTime.TextChanged
        Get_Hours()
    End Sub

    Private Sub msk_outTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_outTime.TextChanged
        Get_Hours()
    End Sub
    Private Sub dtp_InDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_InDateTime.TextChanged
        Get_Hours()
    End Sub
    Private Sub dtp_OutDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_OutDateTime.TextChanged
        Get_Hours()
    End Sub

    Private Sub dtp_OutTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_OutTime.TextChanged
        If New_Entry = True Then
            If IsDate(dtp_OutTime.Text) = True Then
                SetMyCustomFormat()
                msk_outTime.Text = dtp_OutTime.Text
                msk_outTime.SelectionStart = 0
            End If
        End If

    End Sub
    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        ' Dim ps As Printing.PaperSize
        Dim m1 As Integer = 0
        Dim bln As String = ""
        Dim BlNoAr(20) As String
        Dim ItmNm1 As String, ItmNm2 As String
        Dim PpSzSTS As Boolean = False
        Dim strWidth As Single = 0
        Dim i As Integer = 0

        PageWidth = 290 : PrintWidth = 290

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3X30", 300, 3000)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 2
            .Right = 10
            .Top = 5 ' 65
            .Bottom = 5
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        ' PageWidth = 300

        TxtHgt = 14 ' 21 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ' NoofItems_PerPage = 10 '20 

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 10
        ClArr(2) = 120 : ClArr(3) = 40 : ClArr(4) = 40 : ClArr(5) = 45
        ClArr(6) = PrintWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        'ClArr(0) = 0
        'ClArr(1) = 55
        'ClArr(2) = 290 : ClArr(3) = 100 : ClArr(4) = 70 : ClArr(5) = 95
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                '  If Val(prn_HdDt_VAT.Rows(0).Item("CashDiscount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt_VAT.Rows(0).Item("Tax_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt_VAT.Rows(0).Item("Freight_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt_VAT.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0
                    Common_Procedures.MRP_saving = 0

                    CurY = CurY - 5

                    If prn_DetMxIndx > 0 Then

                        Do While DetIndx <= prn_DetMxIndx

                            'If NoofDets > NoofItems_PerPage Then

                            '    CurY = CurY + TxtHgt
                            '    Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            '    NoofDets = NoofDets + 1
                            '    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)
                            '    e.HasMorePages = True

                            '    Return

                            'End If
                            'ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString)
                            'ItmNm2 = ""
                            'If Len(ItmNm1) > 22 Then
                            '    For i = 22 To 1 Step -1
                            '        If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                            '    Next i
                            '    If i = 0 Then i = 22
                            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                            '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
                            'End If

                            CurY = CurY + TxtHgt - 2

                            'If DetIndx <> 1 And Val(prn_DetAr(DetIndx, 1)) <> 0 Then
                            '    CurY = CurY + 2
                            'End If

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 2, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 15, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 1, 0, pFont)

                            'If Val(prn_DetAr(DetIndx, 10)) <> 0 Then
                            '    Common_Procedures.MRP_saving = Common_Procedures.MRP_saving + (Val(prn_DetAr(DetIndx, 10) - prn_DetAr(DetIndx, 7)))
                            'End If

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            DetIndx = DetIndx + 1

                        Loop

                    End If

                    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

                    'If Trim(prn_InpOpts) <> "" Then
                    '    If prn_Count < Len(Trim(prn_InpOpts)) Then

                    '        DetIndx = 1
                    '        prn_PageNo = 0

                    e.HasMorePages = False
                    Return
                    '    End If
                    'End If


                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        ' Dim ptFont As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single

        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""

        Dim CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.sl_no, b.Item_Name, c.Unit_Name, a.Noof_Items, a.Rate, a.Tax_Rate, a.Amount, a.Discount_Perc, a.Discount_Amount, a.Tax_Perc, a.Tax_Amount, a.Total_Amount from Sales_Details a INNER JOIN Item_Head b on a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c on b.unit_idno = c.unit_idno where a.Sales_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), 580 - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        'prn_Count = prn_Count + 1

        'PrintDocument1.DefaultPageSettings.Color = False
        'PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        'e.PageSettings.Color = False

        'prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True

        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE"
        '        ElseIf Val(S) = 3 Then
        '            prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If

        'If Trim(prn_OriDupTri) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), 580 - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If

        ' p1Font = New Font("Calibri", 8, FontStyle.Regular)
        ' Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        'CurY = CurY + TxtHgt '+ 10
        ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, 580, CurY)
        '  LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
        Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        Cmp_Add3 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)
        Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

        'If Trim(Cmp_Add1) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add1), 1) = "," Then
        '        Cmp_Add1 = Trim(Cmp_Add1) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        '    Else
        '        Cmp_Add1 = Trim(Cmp_Add1) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        '    End If
        'Else
        '    Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)
        'End If

        'Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) '& IIf(Trim(prn_HdDt_VAT.Rows(0).Item("Company_Address3").ToString) <> "" And Microsoft.VisualBasic.Right(Trim(prn_HdDt_VAT.Rows(0).Item("Company_Address3").ToString), 1) = ",", " ", ", ") & Trim(prn_HdDt_VAT.Rows(0).Item("Company_Address4").ToString)
        'If Trim(Cmp_Add2) <> "" Then
        '    If Microsoft.VisualBasic.Right(Trim(Cmp_Add2), 1) = "," Then
        '        Cmp_Add2 = Trim(Cmp_Add2) & ", " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        '    Else
        '        Cmp_Add2 = Trim(Cmp_Add2) & " " & Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        '    End If
        'Else
        '    Cmp_Add2 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)
        'End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) <> "" Then
            Cmp_Desc = "(" & Trim(prn_HdDt.Rows(0).Item("Company_Description").ToString) & ")"
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If

        p1Font = New Font("Calibri", 16, FontStyle.Regular)
        pFont = New Font("Calibri", 10, FontStyle.Regular)

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    
        ' CurY = CurY + TxtHgt + 5
        ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, 580, CurY)
        'LnAr(2) = CurY

        Try

            Cen1 = ClAr(1) + ClAr(2)
            W1 = e.Graphics.MeasureString("DATE : ", pFont).Width
            W2 = e.Graphics.MeasureString("Vehicle No :  ", pFont).Width



            CurY = CurY + TxtHgt + 5

            Common_Procedures.Print_To_PrintDocument(e, "-----------------------------------------------------------------------------------------------------------------------------------------", LMargin, CurY + 3, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Token No.", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tocken_No").ToString & " - " & Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Right(Common_Procedures.FnYearCode, 5), 2), LMargin + W2 + 15, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Tocken_Date").ToString), "dd-MM-yyyy"), LMargin + W2 + 15, CurY, 0, 0, p1Font)


            Dim tim As String

            'tim = Format(Now, "hh:mm tt").ToString

            'Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Tocken_Date").ToString), "dd-MM-yyyy") & " ", PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-----------------------------------------------------------------------------------------------------------------------------------------", LMargin, CurY, 0, PrintWidth, pFont)

            W1 = e.Graphics.MeasureString("Vehicle No :  ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_tYPE").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "In Time", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("InDateTime").ToString), "dd-MM-yyyy hh:mm tt"), LMargin + W1 + 15, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Name", LMargin + 5, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_Name").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'End If



            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Address", LMargin + 5, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Address1").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Address2").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'End If


            'CurY = CurY + TxtHgt
            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Address3").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString), LMargin + W1 + 15, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Mobile No", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("mOBILE_nO").ToString), LMargin + W1 + 15, CurY, 0, 0, p1Font)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-----------------------------------------------------------------------------------------------------------------------------------------", LMargin, CurY, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BIKE DAILY PASS INR : 10 For 12 Hours", LMargin + 5, CurY, 2, PageWidth, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "-----------------------------------------------------------------------------------------------------------------------------------------", LMargin, CurY, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Location url : ", LMargin + 5, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.settings.CustomerCode) = "1193" Then
                
                e.Graphics.DrawImage(DirectCast(Image.FromFile(Common_Procedures.AppPath & "\Images\qr_usr_location.PNG"), Drawing.Image), (PageWidth / 2) - 100, CurY, 200, 200)

            End If
            CurY = CurY + 230

            Common_Procedures.Print_To_PrintDocument(e, "--", LMargin + 10, CurY, 2, PageWidth, pFont)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        ' Dim p1Font As Font
        ' Dim I As Integer
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""


        Try



            'CurY = CurY + 50

            'Common_Procedures.Print_To_PrintDocument(e, "--", LMargin + 10, CurY, 2, PageWidth, pFont)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_LedgerName(ByVal Led_Idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim PhAr() As String
        Dim Sno As Integer = 0
        Dim PrcLst_idno As Integer = 0
        Dim Mac_id As Integer = 0
        Dim Grp_idno As Integer = 0
        Dim State_idno As Integer = 0
        Dim Agnt_idno As Integer = 0
        Dim Nr As Integer = 0


        If Trim(cbo_Ledger.Text) = "" Then
            Exit Sub
        End If

        acgrp_idno = 10 'Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)


        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)




        State_idno = 1
        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        Dim LED_ID As Integer = 0

        LED_ID = Led_Idno

        trans = con.BeginTransaction


        Try

            cmd.Transaction = trans

            cmd.Connection = con


            If LED_ID <> 0 Then
                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(cbo_Ledger.Text) & "', Ledger_AlaisName = '', State_Idno = " & Str(Val(State_idno)) & " , LedgerGroup_Idno = " & Str(Val(Grp_idno)) & " , Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = 'BALANCE ONLY', Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_Address2.Text) & "', Ledger_Address3 = '" & Trim(txt_Address3.Text) & "', Ledger_Address4 = '" & Trim(txt_Address4.Text) & "',Ledger_EmailID = '' , Ledger_PhoneNo = '" & Trim(Cbo_MobileNo.Text) & "'  where Ledger_IdNo = " & Str(Val(LED_ID))
                Nr = cmd.ExecuteNonQuery()

            Else
                LED_ID = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)

                If Val(LED_ID) < 101 Then LED_ID = 101

                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4,Ledger_PhoneNo) Values (" & Str(Val(LED_ID)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(cbo_Ledger.Text) & "', " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', 'BALANCE ONLY', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "','" & Trim(Cbo_MobileNo.Text) & "' )"
                Nr = cmd.ExecuteNonQuery()
            End If




            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(LED_ID))
            cmd.ExecuteNonQuery()

            LedArName = Trim(cbo_Ledger.Text)


            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ,Agent_idNo ) Values (" & Str(Val(LED_ID)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", '' ," & Str(Val(Agnt_idno)) & ")"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "delete from Ledger_PhoneNo_Head where Ledger_IdNo = " & Str(Val(LED_ID))
            cmd.ExecuteNonQuery()

            PhAr = Split(Cbo_MobileNo.Text, ",")
            Sno = 0
            For i = 0 To UBound(PhAr)
                If Trim(PhAr(i)) <> "" Then

                    LedPhNo = Trim(PhAr(i))
                    LedPhNo = Replace(LedPhNo, " ", "")
                    LedPhNo = Replace(LedPhNo, "-", "")
                    LedPhNo = Replace(LedPhNo, "_", "")
                    LedPhNo = Replace(LedPhNo, "+", "")
                    LedPhNo = Replace(LedPhNo, "/", "")
                    LedPhNo = Replace(LedPhNo, "\", "")
                    LedPhNo = Replace(LedPhNo, "*", "")

                    If Trim(LedPhNo) <> "" Then
                        Sno = Sno + 1
                        cmd.CommandText = "Insert into Ledger_PhoneNo_Head(Ledger_IdNo, Sl_No, Ledger_PhoneNo) Values (" & Str(Val(LED_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(LedPhNo) & "')"
                        cmd.ExecuteNonQuery()
                    End If

                End If
            Next


            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(LedName)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"

            If New_Entry = True Then new_record()


        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_phoneno_head") > 0 Then
                MessageBox.Show("Duplicate PhoneNo to this ledger", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try


    End Sub

    Private Sub Amount_Calculation()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Rate_Hr As Double
        Dim Rate_Day As Double
        Dim Rate_Month As Double
        Dim TtlHrs As Integer = 0
        Dim TtlHrs_plus As Single = 0
        Dim ModSts As Integer = 0

        Try

            If NoCalc_Status = True Then Exit Sub


            da1 = New SqlClient.SqlDataAdapter("select * from Rate_Head where Company_Idno = " & Str(Val(lbl_Company.Tag)) & "", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                Rate_Hr = dt1.Rows(0).Item("Rate_Per_Hour").ToString
                Rate_Day = dt1.Rows(0).Item("Rate_Per_Day").ToString
                Rate_Month = dt1.Rows(0).Item("Rate_Per_Month").ToString

            End If
            dt1.Clear()

            txt_amount.Text = 0
            If rBtn_Hr.Checked = True Then
                txt_amount.Text = Format(Val(lbl_Total_Hrs.Text) * Val(Rate_Hr), "###############0.00")
                lbl_Rate.Text = Val(Rate_Hr)

            ElseIf rBtn_Daily.Checked = True Then
                lbl_Rate.Text = Val(Rate_Day)

                If IsDate(msk_outTime.Text) = True Then


                    If Val(lbl_Total_Hrs.Text) <> 0 Then

                        If Val(lbl_Total_Hrs.Text) >= 26 Then
                            ' txt_Amount.Text = Format((Val(lbl_Total_Hrs.Text) / 24) * Val(Rate_Day), "################0.00")




                            TtlHrs_plus = (Val(lbl_Total_Hrs.Text) / 26)
                            TtlHrs = (Val(lbl_Total_Hrs.Text) / 26)
                            If (Val(TtlHrs_plus) - Val(TtlHrs)) < 0 Then
                                txt_amount.Text = Format((Val(TtlHrs)) * Val(Rate_Day), "################0.00")
                                lbl_TotalDays.Text = Format(Val(lbl_Total_Hrs.Text) / 24, "##############0")
                            Else
                                txt_amount.Text = Format((Val(TtlHrs) + 1) * Val(Rate_Day), "################0.00")
                                lbl_TotalDays.Text = Format((Val(lbl_Total_Hrs.Text) / 24) + 1, "##############0")
                            End If





                            '  txt_Amount.Text = Format((Val(lbl_Total_Hrs.Text) / 26) * Val(Rate_Day), "################0.00")

                        ElseIf Val(lbl_Total_Hrs.Text) <= 13 Then
                            txt_amount.Text = Format(Val(Rate_Hr), "################0.00")
                        Else
                            txt_amount.Text = Format(Val(Rate_Day), "################0.00")

                            lbl_TotalDays.Text = 1

                        End If

                    Else
                        txt_amount.Text = 0
                        'txt_amount.Text = Format(Val(Rate_Day), "################0.00")
                        lbl_TotalDays.Text = 1
                    End If


                End If

            ElseIf rBtn_Monthly.Checked = True Then

                'If IsDate(msk_inTime.Text) = True And IsDate(msk_outTime.Text) = True Then
                '    lbl_TotalDays.Text = DateDiff(DateInterval.Day, Convert.ToDateTime(msk_inTime.Text), Convert.ToDateTime(msk_outTime.Text))
                '    If Val(lbl_TotalDays.Text) <= 0 Then lbl_TotalDays.Text = 0
                'Else
                '    lbl_TotalDays.Text = 0
                'End If

                'If Val(lbl_TotalDays.Text) > 30 Then

                '    ModSts = Val(lbl_TotalDays.Text) Mod 30

                '    If ModSts <> 0 Then

                '        txt_amount.Text = Format(Val(Rate_Month) * ((Val(lbl_TotalDays.Text) - ModSts) / 30), "###############0.00")

                '    End If



                'Else
                '    txt_amount.Text = Format(Val(Rate_Month), "###############0.00")
                'End If

                txt_amount.Text = 0
                lbl_Rate.Text = Val(Rate_Month)



            End If



        Catch ex As Exception


        Finally

            dt1.Dispose()
            da1.Dispose()


        End Try
    End Sub

    Private Sub rBtn_Hr_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rBtn_Hr.CheckedChanged
        Amount_Calculation()

    End Sub
    Private Sub rBtn_Daily_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rBtn_Daily.CheckedChanged
        lbl_Total_Hrs.Visible = True
        lbl_TotalHrCaption.Visible = True
        lbl_AmountCaption.Visible = True
        txt_amount.Visible = True

        Amount_Calculation()

    End Sub
    Private Sub rBtn_Monthly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rBtn_Monthly.CheckedChanged
        lbl_Total_Hrs.Visible = False
        lbl_TotalHrCaption.Visible = False
        lbl_AmountCaption.Visible = False
        txt_amount.Visible = False
        Get_Hours()
        Amount_Calculation()
    End Sub

    Private Sub lbl_Total_Hrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Total_Hrs.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub lbl_TotalDays_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_TotalDays.TextChanged
        '   Amount_Calculation()
    End Sub

    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click

    End Sub

    Private Sub cbo_VehicleNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.LostFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Vehicle_no As String = ""
        Dim SurName As String = ""
        Dim movno As String = ""

        Try
            Vehicle_no = Trim(cbo_VehicleNo.Text)
            SurName = Common_Procedures.Remove_NonCharacters(Trim(Vehicle_no))

            If rBtn_Monthly.Checked = True Then

                da1 = New SqlClient.SqlDataAdapter("select * from Token_Monthly_Head where Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Sur_Name = '" & Trim(SurName) & "'", con)
                da1.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    If Not IsDBNull(dt1.Rows(0).Item("Close_Status").ToString) Then
                        If Val(dt1.Rows(0).Item("Close_Status").ToString) = 0 Then
                            cbo_Vehicle_type.Text = Trim(dt1.Rows(0).Item("Vehicle_Type").ToString)
                            cbo_Ledger.Text = Trim(dt1.Rows(0).Item("Party_Name").ToString)
                            txt_Address1.Text = Trim(dt1.Rows(0).Item("Party_Address1").ToString)
                            txt_Address2.Text = Trim(dt1.Rows(0).Item("Party_Address2").ToString)
                            txt_Address3.Text = Trim(dt1.Rows(0).Item("Party_Address3").ToString)
                            txt_Address4.Text = Trim(dt1.Rows(0).Item("Party_Address4").ToString)
                            Cbo_MobileNo.Text = Trim(dt1.Rows(0).Item("Party_MobileNo").ToString)
                            lbl_Rate.Text = Val(dt1.Rows(0).Item("Rate").ToString)

                            Get_Hours()
                        Else
                            If MessageBox.Show("Monthly Plan not Found fot this vehilce No" & vbCrLf & "Do you want to create monthly plan for this vehicle No. ?", "NOT FOUND", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = Windows.Forms.DialogResult.Yes Then

                                Dim F4 As New Token_MonthlyPlan_Entry
                                F4.MdiParent = MDIParent1
                                F4.Show()

                                If Val(Common_Procedures.CompIdNo) = 0 Then
                                    F4.Close()
                                    F4.Dispose()
                                End If

                            End If
                        End If
                    End If
                End If
                dt1.Clear()
                da1.Dispose()


            Else
                da1 = New SqlClient.SqlDataAdapter("select Tocken_Type ,OutTime, Tocken_No from Tocken_Head where Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Sur_Name = '" & Trim(SurName) & "'", con)
                da1.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    For I = 0 To dt1.Rows.Count - 1
                        If Not IsDBNull(dt1.Rows(I).Item("OutTime").ToString) Then

                            If InStr(Trim(dt1.Rows(I).Item("OutTime").ToString), "1990") > 0 And Trim(dt1.Rows(I).Item("Tocken_Type").ToString) <> "MONTH" Then
                                movno = dt1.Rows(I).Item("Tocken_No").ToString
                                move_record(movno)
                                cbo_Vehicle_type.Focus()
                                Exit Sub

                            ElseIf Trim(dt1.Rows(I).Item("Tocken_Type").ToString) = "MONTH" Then
                                new_record()
                                rBtn_Monthly.Checked = True
                                cbo_VehicleNo.Text = Trim(Vehicle_no)
                                cbo_Vehicle_type.Focus()
                                Exit Sub
                                'Else
                                '    new_record()
                                '    cbo_Vehicle_type.Focus()
                                '    cbo_VehicleNo.Text = Trim(Vehicle_no)
                                '    Exit Sub
                            End If
                        End If
                    Next
                   
                End If
                dt1.Clear()
                da1.Dispose()

            End If
            

        Catch ex As Exception


        Finally

            dt1.Dispose()
            da1.Dispose()


        End Try
    End Sub
    Private Sub Cbo_MobileNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MobileNo.GotFocus
        If rBtn_Monthly.Checked = True Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Token_Monthly_Head", "Party_MobileNo", "", "")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Tocken_Head", "Mobile_No", "", "")
        End If

    End Sub

    Private Sub Cbo_MobileNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_MobileNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_MobileNo, cbo_Vehicle_type, cbo_Ledger, "Token_Monthly_Head", "Party_MobileNo", "", "")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_MobileNo, cbo_Vehicle_type, cbo_Ledger, "Tocken_Head", "Mobile_No", "", "")
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Cbo_MobileNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_MobileNo.KeyPress
        Try
            If rBtn_Monthly.Checked = True Then
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_MobileNo, cbo_Ledger, "Token_Monthly_Head", "Party_MobileNo", "", "", False)
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_MobileNo, cbo_Ledger, "Tocken_Head", "Mobile_No", "", "", False)
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Cbo_MobileNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MobileNo.LostFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim movno As String = ""

        Try
          
            If Trim(cbo_VehicleNo.Text) <> "" Then Exit Sub

            da1 = New SqlClient.SqlDataAdapter("select top 1 * from Tocken_Head A  LEFT OUTER JOIN  Ledger_Head B ON A.Ledger_Idno = B.Ledger_Idno where Company_Idno = " & Str(Val(lbl_Company.Tag)) & " and Mobile_No = '" & Trim(Cbo_MobileNo.Text) & "' order by  Tocken_Date", con)
                da1.Fill(dt1)

                If dt1.Rows.Count > 0 Then
                    For I = 0 To dt1.Rows.Count - 1
                    If Not IsDBNull(dt1.Rows(I).Item("Vehicle_No").ToString) Then


                        cbo_VehicleNo.Text = dt1.Rows(I).Item("Vehicle_No").ToString
                        cbo_Vehicle_type.Text = dt1.Rows(I).Item("Vehicle_Type").ToString


                        If Trim(dt1.Rows(I).Item("Ledger_Name").ToString) <> "" Then
                            cbo_Ledger.Text = dt1.Rows(I).Item("Ledger_Name").ToString
                        Else
                            cbo_Ledger.Text = dt1.Rows(I).Item("Party_Name").ToString
                        End If


                        If Trim(dt1.Rows(I).Item("Ledger_Address1").ToString) <> "" Then
                            txt_Address1.Text = dt1.Rows(I).Item("Ledger_Address1").ToString
                        Else
                            txt_Address1.Text = dt1.Rows(I).Item("Address1").ToString
                        End If

                        If Trim(dt1.Rows(I).Item("Ledger_Address2").ToString) <> "" Then
                            txt_Address2.Text = dt1.Rows(I).Item("Ledger_Address2").ToString
                        Else
                            txt_Address2.Text = dt1.Rows(I).Item("Address2").ToString
                        End If


                        If Trim(dt1.Rows(I).Item("Ledger_Address3").ToString) <> "" Then
                            txt_Address3.Text = dt1.Rows(I).Item("Ledger_Address3").ToString
                        Else
                            txt_Address3.Text = dt1.Rows(I).Item("Address3").ToString
                        End If


                        If Trim(dt1.Rows(I).Item("Ledger_Address4").ToString) <> "" Then
                            txt_Address4.Text = dt1.Rows(I).Item("Ledger_Address4").ToString
                        Else
                            txt_Address4.Text = dt1.Rows(I).Item("Address4").ToString
                        End If


                        If Trim(dt1.Rows(I).Item("Ledger_PhoneNo").ToString) <> "" Then
                            Cbo_MobileNo.Text = dt1.Rows(I).Item("Ledger_PhoneNo").ToString
                        Else
                            Cbo_MobileNo.Text = dt1.Rows(I).Item("Mobile_No").ToString
                        End If
                    End If
                    Next

                End If
                dt1.Clear()
                da1.Dispose()

           

        Catch ex As Exception


        Finally

            dt1.Dispose()
            da1.Dispose()


        End Try
    End Sub

    Private Sub txt_amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_amount.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If

        End If


    End Sub
End Class
