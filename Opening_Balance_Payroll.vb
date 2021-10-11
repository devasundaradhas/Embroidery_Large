Public Class Opening_Balance_Payroll
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private ClrSTS As Boolean = False
  
    Private Sub clear()

        ClrSTS = True

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Name.Text = ""
        txt_OpnAmountSalAdvance.Text = "0.00"
        cbo_CrDrType.Text = "Cr"

        txt_OpnSalaryForBonus.Text = ""
        txt_OpnCL_Leaves.Text = ""
        txt_OpnML_Leaves.Text = ""
        txt_OpnWeekOff_Credits.Text = ""
        txt_OpnAmountSalAdvance.Enabled = True
        cbo_CrDrType.Enabled = True
     

        ClrSTS = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dgvtxtedtctrl As DataGridViewTextBoxEditingControl

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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


        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
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


    Private Sub move_record(ByVal idno As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
    
        Dim Sign As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Head a Where a.Employee_IdNo = " & Str(Val(idno)) & "", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_IdNo.Text = dt1.Rows(0).Item("Employee_IdNo").ToString
                cbo_Name.Text = dt1.Rows(0).Item("Employee_Name").ToString

                txt_OpnSalaryForBonus.Text = Format(Val(dt1.Rows(0).Item("Opening_SalaryFor_Bonus").ToString), "##########0.00")
                txt_OpnCL_Leaves.Text = Val(dt1.Rows(0).Item("Opening_CL_Leaves").ToString)
                txt_OpnML_Leaves.Text = Val(dt1.Rows(0).Item("Opening_ML_Leaves").ToString)
                txt_OpnWeekOff_Credits.Text = Val(dt1.Rows(0).Item("Opening_WeekOff_Credits").ToString)

                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        If Val(dt2.Rows(0).Item(0).ToString) <> 0 Then
                            txt_OpnAmountSalAdvance.Text = Trim(Format(Math.Abs(Val(dt2.Rows(0).Item(0).ToString)), "#########0.00"))
                        End If
                        If Val(dt2.Rows(0).Item(0).ToString) >= 0 Then
                            cbo_CrDrType.Text = "Cr"
                        Else
                            cbo_CrDrType.Text = "Dr"
                        End If
                    End If
                End If
                dt2.Clear()

            End If
            dt1.Clear()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            'If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

        End Try



    End Sub

    Private Sub Opening_Stock_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Opening_Stock_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Dt7 As New DataTable
        Dim Dt8 As New DataTable
        Dim Dt9 As New DataTable
        Dim dttm As DateTime

        Me.Text = ""

        dttm = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
        lbl_Heading.Text = "OPENING BALANCE    -    AS ON  :  " & dttm.ToShortDateString

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
        da.Fill(Dt1)
        cbo_Name.DataSource = Dt1
        cbo_Name.DisplayMember = "Employee_Name"

  
      

        AddHandler cbo_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpnAmountSalAdvance.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CrDrType.GotFocus, AddressOf ControlGotFocus
       
        AddHandler txt_OpnSalaryForBonus.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpnCL_Leaves.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpnML_Leaves.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpnWeekOff_Credits.GotFocus, AddressOf ControlGotFocus
       
        AddHandler cbo_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpnAmountSalAdvance.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CrDrType.LostFocus, AddressOf ControlLostFocus
     
        AddHandler txt_OpnSalaryForBonus.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpnCL_Leaves.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpnML_Leaves.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpnWeekOff_Credits.LostFocus, AddressOf ControlLostFocus
       
        AddHandler txt_OpnAmountSalAdvance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OpnSalaryForBonus.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OpnCL_Leaves.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OpnML_Leaves.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OpnWeekOff_Credits.KeyDown, AddressOf TextBoxControlKeyDown
    
        AddHandler txt_OpnAmountSalAdvance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OpnSalaryForBonus.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OpnCL_Leaves.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OpnWeekOff_Credits.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OpnML_Leaves.KeyPress, AddressOf TextBoxControlKeyPress

        'cbo_Ledger.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Opening_Stock_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Opening_Stock_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        '  Dim da As SqlClient.SqlDataAdapter
        '  Dim dt As DataTable
        Dim NewCode As String
        Dim LedName As String

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Employee_Salary_Advance_Opening, "~L~") = 0 And InStr(Common_Procedures.UR.Employee_Salary_Advance_Opening, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        LedName = Common_Procedures.Employee_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

        'da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 OR Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0) ", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Pavu Delivered (or) Production Entered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        'da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("BabyCone Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        'da = New SqlClient.SqlDataAdapter("select count(*) from voucher_bill_head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount <> (credit_amount + debit_amount) ", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Alrady Amount Received/Paid for some bills", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
                Exit Sub
            End If

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from PayRoll_Employee_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Employee_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()
          
            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Name.Enabled = True And cbo_Name.Visible = True Then cbo_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Employee_IdNo from PayRoll_Employee_Head where Employee_IdNo <> 0 Order by Employee_IdNo"
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

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            da = New SqlClient.SqlDataAdapter("select top 1 Employee_IdNo from PayRoll_Employee_Head where Employee_IdNo > " & Str(OrdByNo) & " Order by Employee_IdNo", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 Employee_IdNo from PayRoll_Employee_Head where Employee_IdNo < " & Str(Val(OrdByNo)) & " Order by Employee_IdNo desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Employee_IdNo from PayRoll_Employee_Head where Employee_IdNo <> 0 Order by Employee_IdNo desc", con)
        Dim dt As New DataTable
        Dim movno As Integer

        Try
            da.Fill(dt)

            movno = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select max(Employee_IdNo) from PayRoll_Employee_Head where Employee_IdNo <> 0", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(NewID) <= 100 Then NewID = 100

            lbl_IdNo.Text = Val(NewID) + 1

            lbl_IdNo.ForeColor = Color.Red

            If cbo_Name.Enabled And cbo_Name.Visible Then cbo_Name.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LedName As String
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouAmt As Double
        
        Dim Led_ID As Integer
      
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Double = 0
       
        Dim vAgt_ID As Integer = 0

        Dim bl_amt As Double = 0
        Dim CrDr_Amt_ColNm As String = ""
        Dim vou_bil_no As String = ""
        Dim vou_bil_code As String = ""
        Dim Led_Type As String = ""
        Dim StkOf_IdNo As Integer = 0
        Dim LedTyp As String = ""
      
        Dim BnStk_IdNo As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Employee_Salary_Advance_Opening, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_Name.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Name.Enabled Then cbo_Name.Focus()
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Name.Enabled Then cbo_Name.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_Name.Text))

        LedName = Common_Procedures.Employee_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Name.Enabled Then cbo_Name.Focus()
            Exit Sub
        End If

        If Val(txt_OpnAmountSalAdvance.Text) <> 0 And Trim(cbo_CrDrType.Text) = "" Then
            MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CrDrType.Enabled Then cbo_CrDrType.Focus()
            Exit Sub
        End If

      

        tr = con.BeginTransaction

        Try
            Led_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            If Val(txt_OpnAmountSalAdvance.Text) <> 0 Then

                VouAmt = Math.Abs(Val(txt_OpnAmountSalAdvance.Text))
                If Trim(UCase(cbo_CrDrType.Text)) = "DR" Then VouAmt = -1 * VouAmt

                Sno = Sno + 1

                cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Sl_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(lbl_IdNo.Text)) & ", 'Opng', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(VouAmt)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "')"
                Nr = cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Update PayRoll_Employee_Head set  Opening_SalaryFor_Bonus = " & Str(Val(txt_OpnSalaryForBonus.Text)) & ",Opening_CL_Leaves = " & Str(Val(txt_OpnCL_Leaves.Text)) & "  , Opening_ML_Leaves =" & Str(Val(txt_OpnML_Leaves.Text)) & "   , Opening_WeekOff_Credits = " & Str(Val(txt_OpnWeekOff_Credits.Text)) & " where Employee_IdNo =  " & Str(Val(lbl_IdNo.Text)) & " "
            Nr = cmd.ExecuteNonQuery()

            
            tr.Commit()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


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
            tr.Rollback()
            
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)


        End Try

        If cbo_Name.Enabled And cbo_Name.Visible Then cbo_Name.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CrDrType.KeyDown
        Try
            With cbo_CrDrType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpnAmountSalAdvance.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpnCL_Leaves.Focus()
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CrDrType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_CrDrType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .GetItemText(.SelectedItem)
                                    '.Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        txt_OpnCL_Leaves.Focus()

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

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Name, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        With cbo_Name
            If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If txt_OpnAmountSalAdvance.Enabled And txt_OpnAmountSalAdvance.Visible Then
                    txt_OpnAmountSalAdvance.Focus()
                Else
                    txt_OpnSalaryForBonus.Focus()
                End If

            End If
        End With

    End Sub


    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Name.KeyPress
        Dim LedIdNo As Integer

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Name, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            LedIdNo = Common_Procedures.Employee_NameToIdNo(con, cbo_Name.Text)
            If Val(LedIdNo) <> 0 Then
                move_record(LedIdNo)
            End If
            txt_OpnAmountSalAdvance.Enabled = True
            cbo_CrDrType.Enabled = True

            If txt_OpnAmountSalAdvance.Enabled And txt_OpnAmountSalAdvance.Visible Then
                txt_OpnAmountSalAdvance.Focus()
            Else
                txt_OpnSalaryForBonus.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class