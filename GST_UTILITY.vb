Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail
Imports System.IO.Ports
Imports System.Windows.Forms.DataVisualization.Charting
Imports Newtonsoft.Json.Linq.JObject
Public Class GST_UTILITY

    Public Shared responseFromServer As String
    Public Shared msg As String
    Public Shared status As String
    Public Shared Function Get_GSTIN_Reg_Info(GSTIN As String, Optional State_Code As String = "", Optional StateName As String = "") As String

        Get_GSTIN_Reg_Info = "Invalid GSTIN@#$ "

        Dim GSTIN_A As String = Trim(GSTIN)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        If Len(GSTIN_A) <> 15 Then
            Return "Invalid GSTIN@#$Invalid Length"
            Exit Function
        End If

        If Not IsNumeric(Microsoft.VisualBasic.Left(GSTIN_A, 2)) Then
            Return "Invalid GSTIN@#$Invalid State Code"
            Exit Function
        End If

        If Len(State_Code) = 2 Then
            If Microsoft.VisualBasic.Left(GSTIN_A, 2) <> State_Code Then
                Return "Invalid GSTIN@#$State Code Mismatch (Internal Check)"
                Exit Function
            End If
        End If

        For i = 3 To 7
            If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN_A, i, 1)) Then
                Return "Invalid GSTIN@#$Invalid PAN Format"
                Exit Function
            End If
        Next

        For i = 8 To 11
            If Not IsNumeric(Microsoft.VisualBasic.Mid(GSTIN_A, i, 1)) Then
                Return "Invalid GSTIN@#$Invalid PAN Format"
                Exit Function
            End If
        Next

        If IsNumeric(Microsoft.VisualBasic.Mid(GSTIN_A, 12, 1)) Then
            Return "Invalid GSTIN@#$Invalid PAN Format"
            Exit Function
        End If

        Try

            Dim aspid As String = ""
            Dim asppwd As String = ""

            Dim Cn As New SqlClient.SqlConnection("Data Source=ddbiz1.cxyfb9k80u6z.ap-south-1.rds.amazonaws.com;Initial Catalog=tsoft_onlinebilling_CompanyGroup_Details;User ID=dani;Password=Deva_1983;Integrated Security=False;Connect Timeout=60")
            Cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("Select * from TaxPro", Cn)
            Dim dt As New DataTable

            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If Not IsDBNull(dt.Rows(0).Item("aspid")) Then
                    aspid = dt.Rows(0).Item("aspid")
                End If

                If Not IsDBNull(dt.Rows(0).Item("password")) Then
                    asppwd = dt.Rows(0).Item("password")
                End If

            Else

                Return ("Could'nt execute validation,ASP Credetials not found")

            End If

            If Len(Trim(aspid)) = 0 Or Len(Trim(asppwd)) = 0 Then
                Return ("Could'nt execute validation,ASP Credetials not found")
            End If


            Dim request As WebRequest =
                WebRequest.Create("https://gstapi.charteredinfo.com/commonapi/v1.1/search?aspid=" & aspid & "&password=" & asppwd & "&Action=TP&Gstin=" & GSTIN)
            'Dim request As WebRequest =
            'WebRequest.Create("https://gstapi.charteredinfo.com/commonapi/v1.1/search?aspid=" & aspid & "&password=" & asppwd & "&Action=TP&Gstin=32AXFPT6438B1ZJ")
            request.Credentials = CredentialCache.DefaultCredentials


            Dim response As WebResponse = request.GetResponse()

            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            responseFromServer = reader.ReadToEnd()
            reader.Close()
            response.Close()

            Return ("Valid GSTIN@#$" + responseFromServer)

            Exit Function

        Catch ex As Exception

            Return ("Invalid GSTIN@#$" & ex.Message)
            Exit Function

        End Try

    End Function

End Class
