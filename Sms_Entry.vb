Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail

Public Class Sms_Entry
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Public Shared vSmsPhoneNo As String
    Public Shared vSmsMessage As String
    Public Shared SMSProvider_SenderID As String
    Public Shared SMSProvider_Key As String
    Public Shared SMSProvider_RouteID As String
    Public Shared SMSProvider_Type As String
    Public Shared vSmsSendStatus As String
    Public Shared vSmsSendFor As String

    Private Sub Sms_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 50 - Me.Width
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 250 - Me.Height

        lbl_Date.Visible = False
        dtp_Date.Visible = False

        lbl_Heading.Text = "MESSAGE"
        If Trim(vSmsSendFor) = "WEDDING" Then
            lbl_Date.Visible = True
            dtp_Date.Visible = True
            lbl_Heading.Text = "WEDDING DAY MESSAGE"
        ElseIf Trim(vSmsSendFor) = "BIRTHDAY" Then
            lbl_Date.Visible = True
            dtp_Date.Visible = True
            lbl_Heading.Text = "BIRTHDAY MESSAGE"
        End If

        If Trim(vSmsSendStatus) = "" Then
            vSmsSendStatus = "SINGLE"
        End If

        If Trim(vSmsSendStatus) = "ALL" Then
            txt_PhnNo.Enabled = False
        Else
            txt_PhnNo.Enabled = True
        End If

        txt_PhnNo.Text = Trim(vSmsPhoneNo)
        txt_Msg.Text = Trim(vSmsMessage)
    End Sub

    Private Sub Sms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub btnSendSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim timeout As Integer = 50000
        Dim da As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        ' Dim n As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            If IsDate(dtp_Date.Value) = True Then
                If Trim(vSmsSendFor) = "WEDDING" Then
                    Condt = "  day(a.Wedding_Date) = " & Val(dtp_Date.Value.Day) & " and month(a.Wedding_Date) = " & Val(dtp_Date.Value.Month) & " "
                ElseIf Trim(vSmsSendFor) = "BIRTHDAY" Then
                    Condt = "  day(a.Birth_Date) = " & Val(dtp_Date.Value.Day) & " and month(a.Birth_Date) = " & Val(dtp_Date.Value.Month) & " "
                    'Condt = "  a.Birth_Date = '" & Trim(Format(dtp_Date.Value, "dd/MM")) & "'"
                Else
                    Condt = ""
                End If
            End If

            smstxt = Trim(txt_Msg.Text)

            If Trim(vSmsSendStatus) = "ALL" Then

                PhNo = ""
                '--------------
                da = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Head a  where A.ledger_idno > 100 " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Ledger_IdNo ", con)
                dt = New DataTable
                da.Fill(dt)


                If dt.Rows.Count > 0 Then

                    For i = 0 To dt.Rows.Count - 1

                        PhNo = Trim(dt.Rows(i).Item("Ledger_PhoneNo").ToString)

                        If Trim(PhNo) <> "" Then
                            Threading.Thread.Sleep(500)

                            url = "	http://198.24.149.4/API/pushsms.aspx?loginID=tsoft&password=amutha&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"



                            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(SMSProvider_Key) & "&routeid=" & Trim(SMSProvider_RouteID) & "&type=" & Trim(SMSProvider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(SMSProvider_SenderID) & "&msg=" & Trim(smstxt)


                            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(Common_Procedures.settings.SMS_Provider_Key) & "&routeid=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID) & "&type=" & Trim(Common_Procedures.settings.SMS_Provider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&msg=" & Trim(smstxt)

                            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=134&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

                            '--url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

                            '--(jenilla)
                            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=" & Trim(PhNo) & "&source=JENIAL&message=" & Trim(smstxt)

                            '--THIS IS Working (jenilla)
                            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=" & Trim(smstxt)

                            'THIS IS OK
                            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

                            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

                            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

                            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

                            request = DirectCast(WebRequest.Create(url), HttpWebRequest)
                            request.KeepAlive = True

                            request.Timeout = timeout

                            'request.Method = (typ == RequestType.GET ? "GET" : "POST")
                            'request.Accept = "*/*"
                            'request.Headers.Add(HttpRequestHeader.AcceptLanguage, "de")
                            'request.Headers.Add("UA-CPU", "x86")
                            'request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate")
                            'request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618) "
                            'request.ContentType = "application/x-www-form-urlencoded"

                            response = DirectCast(request.GetResponse(), HttpWebResponse)

                            'MessageBox.Show("Response StatusCode : " & response.StatusCode)
                            'MessageBox.Show("Response StatusDescription : " & response.StatusDescription)
                            If Trim(UCase(response.StatusDescription)) = "OK" Then
                                '   MessageBox.Show("Sucessfully Sent...")

                                'MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                'MessageBox.Show("Response: " & response.StatusDescription)
                            Else
                                MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If

                            response.Close()

                            response = Nothing
                            request = Nothing

                        End If
                    Next i

                End If

                MessageBox.Show("All Messages Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)

                '--------------



            Else

                PhNo = Trim(txt_PhnNo.Text)

                url = "	http://198.24.149.4/API/pushsms.aspx?loginID=tsoft&password=amutha&mobile=" & Trim(PhNo) & "&text=" & Trim(smstxt) & "&senderid=" & Trim(SMSProvider_SenderID) & "&route_id=2&Unicode=0"


                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(SMSProvider_Key) & "&routeid=" & Trim(SMSProvider_RouteID) & "&type=" & Trim(SMSProvider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(SMSProvider_SenderID) & "&msg=" & Trim(smstxt)

                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=" & Trim(Common_Procedures.settings.SMS_Provider_Key) & "&routeid=" & Trim(Common_Procedures.settings.SMS_Provider_RouteID) & "&type=" & Trim(Common_Procedures.settings.SMS_Provider_Type) & "&contacts=" & Trim(PhNo) & "&senderid=" & Trim(Common_Procedures.settings.SMS_Provider_SenderID) & "&msg=" & Trim(smstxt)

                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=134&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

                '--url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=" & Trim(PhNo) & "&senderid=WEBSMS&msg=" & Trim(smstxt)

                '--(jenilla)
                'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=" & Trim(PhNo) & "&source=JENIAL&message=" & Trim(smstxt)

                '--THIS IS Working (jenilla)
                'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=" & Trim(smstxt)

                'THIS IS OK
                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

                'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

                'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

                'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

                request = DirectCast(WebRequest.Create(url), HttpWebRequest)
                request.KeepAlive = True

                request.Timeout = timeout

                'request.Method = (typ == RequestType.GET ? "GET" : "POST")
                'request.Accept = "*/*"
                'request.Headers.Add(HttpRequestHeader.AcceptLanguage, "de")
                'request.Headers.Add("UA-CPU", "x86")
                'request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate")
                'request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618) "
                'request.ContentType = "application/x-www-form-urlencoded"

                response = DirectCast(request.GetResponse(), HttpWebResponse)

                'MessageBox.Show("Response StatusCode : " & response.StatusCode)
                'MessageBox.Show("Response StatusDescription : " & response.StatusDescription)
                If Trim(UCase(response.StatusDescription)) = "OK" Then
                    MessageBox.Show("Sucessfully Sent...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'MessageBox.Show("Response: " & response.StatusDescription)
                Else
                    MessageBox.Show("Failed to sent SMS...", "FOR SENDING SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                response.Close()

                response = Nothing
                request = Nothing

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally


        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class