Public Class Change_Period
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '------
    End Sub

    Private Sub Change_Period_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.Open()

        txt_FromYear.Text = Year(Common_Procedures.Company_FromDate)
        lbl_ToYear.Text = Year(Common_Procedures.Company_ToDate)

    End Sub

    Private Sub txt_FromYear_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FromYear.GotFocus
        With txt_FromYear
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_FromYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FromYear.LostFocus
        With txt_FromYear
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub btn_ChangePeriod_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.GotFocus
        With btn_ChangePeriod
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_ChangePeriod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.LostFocus
        With btn_ChangePeriod
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub

    Private Sub btn_close_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.GotFocus
        With btn_close
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_close_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.LostFocus
        With btn_close
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub

    Private Sub txt_FromYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FromYear.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_FromYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_FromYear.TextChanged
        lbl_ToYear.Text = ""
        If Val(txt_FromYear.Text) <> 0 And Len(Trim(txt_FromYear.Text)) >= 4 Then
            lbl_ToYear.Text = Val(txt_FromYear.Text) + 1
        End If
    End Sub

    Private Sub btn_ChangePeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim st_year As Integer = 0, en_year As Integer = 0
        Dim amt As Double = 0

        If Val(txt_FromYear.Text) = 0 Or Len(Trim(txt_FromYear.Text)) < 4 Then
            MessageBox.Show("Invalid From Year", "DOES NOT CHANGE YEAR...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_FromYear.Enabled And txt_FromYear.Visible Then txt_FromYear.Focus()
            Exit Sub
        End If

        If Val(lbl_ToYear.Text) = 0 Or Len(Trim(lbl_ToYear.Text)) < 4 Then
            MessageBox.Show("Invalid To Year", "DOES NOT CHANGE YEAR...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_FromYear.Enabled And txt_FromYear.Visible Then txt_FromYear.Focus()
            Exit Sub
        End If

        st_year = Val(Microsoft.VisualBasic.Left(Common_Procedures.FnRange, 4))
        en_year = Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)) - 1

        If Not (Val(txt_FromYear.Text) >= st_year And Val(txt_FromYear.Text) <= en_year + 1) Then
            MessageBox.Show("Invalid Financial Year", "DOES NOT CHANGE YEAR...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_FromYear.Enabled And txt_FromYear.Visible Then txt_FromYear.Focus()
            Exit Sub
        End If


        If Val(txt_FromYear.Text) < st_year Or Val(txt_FromYear.Text) > en_year Then

            If MessageBox.Show("Do you want to Create New year?", "FOR NEW PERIOD CREATION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                If Year(Common_Procedures.Company_ToDate) <> Val(txt_FromYear.Text) Then
                    MessageBox.Show("You can create a new year from the last year only", "DOES NOT CHANGE YEAR...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If txt_FromYear.Enabled And txt_FromYear.Visible Then txt_FromYear.Focus()
                    Exit Sub
                End If

                If Val(txt_FromYear.Text) < st_year Then st_year = Val(txt_FromYear.Text)

                en_year = en_year + 1

                If Val(lbl_ToYear.Text) > en_year Then en_year = Val(lbl_ToYear.Text)

                Common_Procedures.FnRange = Trim(st_year) & "-" & Trim(en_year)
                Common_Procedures.CompGroupFnRange = Common_Procedures.FnRange
                Common_Procedures.Company_ToDate = Format(Common_Procedures.Company_ToDate, "dd/MM/") & Trim(en_year)

                cmd.Connection = con

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@CompanyToDate", Common_Procedures.Company_ToDate)

                cmd.CommandText = "Truncate table FinancialRange_Head"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "insert into FinancialRange_Head ( Financial_Range ) Values ('" & Trim(Common_Procedures.FnRange) & "')"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head set Financial_Range = '" & Trim(Common_Procedures.FnRange) & "', To_Date = @CompanyToDate Where CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo))
                cmd.ExecuteNonQuery()

            End If

        End If

        Common_Procedures.FnYearCode = Trim(Microsoft.VisualBasic.Right(Val(txt_FromYear.Text), 2)) & "-" & Trim(Microsoft.VisualBasic.Right(Val(txt_FromYear.Text) + 1, 2))
        Common_Procedures.Company_FromDate = Format(Common_Procedures.Company_FromDate, "dd/MM/") & Trim(Val(txt_FromYear.Text))
        Common_Procedures.Company_ToDate = Format(Common_Procedures.Company_ToDate, "dd/MM/") & Trim(Val(lbl_ToYear.Text))

        MdiParent.Text = Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")         -          " & Common_Procedures.Company_FromDate & "  TO  " & Common_Procedures.Company_ToDate

        'If Trim(UCase(Company_Or_CompanyGroup)) = "COMPANY GROUP" Then
        '    Call set_MdiFormCaption(CmpGrpDet.Name, CmpDet.FnRange, CmpDet.FromDate, CmpDet.ToDate)
        'Else
        '    Call set_MdiFormCaption(CmpDet.Name, CmpDet.FnRange, CmpDet.FromDate, CmpDet.ToDate)
        'End If

        'If Year(Common_Procedures.Company_FromDate) = Val(Microsoft.VisualBasic.(Common_Procedures.FnRange, 4)) Then
        '    OpeningBalance_Visibility = True
        'Else
        '    OpeningBalance_Visibility = False
        'End If

        'Call AccountsVoucher_Posting_For_ProfitAndLoss(con)

        Call Common_Procedures.AccountsVoucher_Posting_For_ProfitAndLoss()

        btn_close_Click(sender, e)

    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

End Class