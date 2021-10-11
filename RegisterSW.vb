Imports System.Management

'Imports System.Net
'Imports System.Net.FtpWebRequest

Imports System.IO

'Imports System.Net.Sockets

Public Class RegisterSW

    Public Shared LastSysTime As DateTime

    Private Sub btn_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Register.Click

        Dim HDserial As String = ""
        Dim Code As Date = "1-1-1900"
        Code = CDate(Authenticate.RevertAuthenticationCode(txt_OTP.Text))

        ' MsgBox(DateDiff(DateInterval.Minute, Now, Code))

        If DateDiff(DateInterval.Minute, Now, Code) >= -10 And DateDiff(DateInterval.Minute, Now, Code) <= 10 Then

            Dim CONNFILE As String = Application.StartupPath & "\LOGINPARAMETERS.TXT"

            Try
                If (System.IO.File.Exists(CONNFILE)) Then
                    System.IO.File.Delete(CONNFILE)
                End If

                If IO.File.Exists(CONNFILE) Then
                    IO.File.Delete(CONNFILE)
                End If

                Dim TMPSTR As String

                Using sw As New IO.StreamWriter(IO.File.Open(CONNFILE, IO.FileMode.Create))

                    Do
                        TMPSTR = Authenticate.AuthenticationCode(My.Computer.Name & "\TSOFT")
                        If Authenticate.RevertAuthenticationCode(TMPSTR) = My.Computer.Name & "\TSOFT" Then
                            sw.WriteLine(TMPSTR)
                            Exit Do
                        End If
                        TMPSTR = ""
                    Loop

                    Do
                        TMPSTR = Authenticate.AuthenticationCode("tsoftsql")
                        If Authenticate.RevertAuthenticationCode(TMPSTR) = "tsoftsql" Then
                            sw.WriteLine(TMPSTR)
                            Exit Do
                        End If
                        TMPSTR = ""
                    Loop


                    Do
                        TMPSTR = Authenticate.AuthenticationCode("tsoft_billing_1")
                        If Authenticate.RevertAuthenticationCode(TMPSTR) = "tsoft_billing_1" Then
                            sw.WriteLine(TMPSTR)
                            Exit Do
                        End If
                        TMPSTR = ""
                    Loop


                    'Dim mos As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia  Where NOT Removable = 'TRUE'")

                    'For Each mo As ManagementObject In mos.Get()
                    '    If InStr(mo.Path.ToString, "PHYSICALDRIVE0") > 0 Then
                    '        Dim serial As String = mo("SerialNumber").ToString()
                    '        HDserial = mo("SerialNumber").ToString()
                    '        GoTo b
                    '    End If
                    'Next


                    Dim fso As Object = CreateObject("Scripting.FileSystemObject")
                    Dim Drv As Object = fso.GetDrive(fso.GetDriveName(Application.StartupPath))
                    With Drv
                        If .IsReady Then
                            HDserial = .SerialNumber.ToString
                        Else    '"Drive Not Ready!"
                            HDserial = ""
                        End If
                    End With

                    If Len(Trim(HDserial)) = 0 Then
                        MsgBox("Could Not Read Hard Disk Serial Number. Contact Supplier")
                        Application.Exit()
                    End If

B:

                    Dim expdate As Date
                    expdate = dtp_ValidUpto.Value
                    Dim AttempCount As Integer = 0

                    Do

                        TMPSTR = Authenticate.AuthenticationCode(HDserial + "$$$" + Format(expdate, "dd-MMM-yyyy"))
                        If Authenticate.RevertAuthenticationCode(TMPSTR) = HDserial + "$$$" + Format(expdate, "dd-MMM-yyyy") Then
                            sw.WriteLine(TMPSTR)
                            Exit Do
                        End If

                        TMPSTR = ""
                        AttempCount = AttempCount + 1

                        If AttempCount = 5 Then
                            MsgBox("Error in setting Validity Date")
                            Exit Sub
                        End If
                    Loop

COMP:

                    sw.Close()

                End Using

                MsgBox("REGISTRATION SUCCEEDS.PLEASE RE-START THE APPLICATION")
                Application.Exit()

            Catch EX As Exception

                MsgBox(EX.Message & ". REGISTRATION FAILS")
                Application.Exit()

            End Try

            '------------------

        Else

            MsgBox("INVALID OTP")
            Application.Exit()

        End If

    End Sub

    Private Sub RegisterSW_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    'Public Function GetNISTTime(ByVal host As String) As DateTime

    '    Dim timeStr As String

    '    Try
    '        Dim reader As New StreamReader(New TcpClient(host, 13).GetStream)
    '        LastSysTime = DateTime.UtcNow()
    '        timeStr = reader.ReadToEnd()
    '        reader.Close()

    '        Dim jd As Integer = Integer.Parse(timeStr.Substring(1, 5))
    '        Dim yr As Integer = Integer.Parse(timeStr.Substring(7, 2))
    '        Dim mo As Integer = Integer.Parse(timeStr.Substring(10, 2))
    '        Dim dy As Integer = Integer.Parse(timeStr.Substring(13, 2))
    '        Dim hr As Integer = Integer.Parse(timeStr.Substring(16, 2))
    '        Dim mm As Integer = Integer.Parse(timeStr.Substring(19, 2))
    '        Dim sc As Integer = Integer.Parse(timeStr.Substring(22, 2))
    '        Dim Temp As Integer = CInt(AscW(timeStr(7)))

    '        Return New DateTime(yr + 2000, mo, dy, hr, mm, sc)

    '        'Catch ex As SocketException
    '        '    MsgBox(ex.Message)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try



    'End Function

End Class