Imports Microsoft.Reporting.WinForms.ReportViewer
Imports System.IO
Imports Amazon.S3
Imports Amazon.S3.Model
Imports Amazon.Runtime
Imports Amazon
Imports Amazon.S3.Util
Imports Amazon.S3.Transfer
Imports Amazon.DynamoDBv2
'Imports Amazon.AWSConfigsDynamoDB

Module ProcessReport

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Const AWS_ACCESS_KEY As String = "AKIAQDQ3Z6TGT5ZHUUEP"
    Const AWS_SECRET_KEY As String = "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"

    Private Property s3Client As Amazon.S3.IAmazonS3

    Public Sub ProcessReport()

        Try

            s3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.APSouth1)

        Catch ex As Exception

        End Try

        con.Open()

        Dim da As SqlClient.SqlDataAdapter
        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = con
        Dim dtbl1 As DataTable
        Dim Bal As Single

        Dim RV As New Microsoft.Reporting.WinForms.ReportViewer

        da = New SqlClient.SqlDataAdapter("select  '' as Company_Name, '' as Company_Address1, '' as Company_Address2, '' as Report_Heading1, '' as Report_Heading2, '' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6, Name7 from reporttemp Order by Date1, Meters5, Int5, meters1, name2, name1", con)
        dtbl1 = New DataTable
        da.Fill(dtbl1)

        Bal = 0
        If dtbl1.Rows.Count > 0 Then
            Bal = Val(dtbl1.Rows(0).Item("Currency1").ToString) - Val(dtbl1.Rows(0).Item("Currency2").ToString)
            dtbl1.Rows(0).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
            For i = 1 To dtbl1.Rows.Count - 1
                Bal = Val(Bal) + Val(dtbl1.Rows(i).Item("Currency1").ToString) - Val(dtbl1.Rows(i).Item("Currency2").ToString)
                dtbl1.Rows(i).Item("Name6") = Trim(Format(Math.Abs(Val(Bal)), "#########0.00")) & IIf(Val(Bal) >= 0, " Dr", " Cr")
            Next i
        End If

        If dtbl1.Rows.Count = 0 Then

            cmd.CommandText = "Insert into reporttemp(Currency12) values (-9999)"
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("select  '' as Company_Name, '' as Company_Address1, '' as Company_Address2, '' as Report_Heading1, '' as Report_Heading2, '' as Report_Heading3, Int5, Date1, Meters1, Name1, Name2, Name3, Currency1, Currency2, Name6, Name4, Name5, Meters6 from reporttemp Order by Int5, Date1, meters1, name2, name1", con)
            dtbl1 = New DataTable
            da.Fill(dtbl1)

        End If

        Dim warnings As Microsoft.Reporting.WinForms.Warning()
        Dim streamIds As String() = {}
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty

        Dim RpDs1 As Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1 = New Microsoft.Reporting.WinForms.ReportDataSource
        RpDs1.Name = "DataSet1"
        RpDs1.Value = dtbl1

        RV.LocalReport.ReportPath = Trim(Common_Procedures.AppPath) & "\Reports\Report_SingleLedger.rdlc"
        RV.LocalReport.DataSources.Clear()

        RV.LocalReport.DataSources.Add(RpDs1)

        RV.LocalReport.Refresh()
        RV.RefreshReport()


        'Dim bytes As Byte() = RV.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamIds, warnings)

        Dim bytes As Byte() = RV.LocalReport.Render("PDF", Nothing, mimeType, encoding, ".pdf", streamIds, warnings)
        Dim fs As New MemoryStream(bytes)

        'fs = New FileStream("D:\output.pdf", FileMode.Create)
        'fs.Write(bytes, 0, bytes.Length)

        'End Using

        Dim target As String = Path.GetTempPath()
        Dim returnval As String = ""


        Try

            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, "novareportfiles") Then
                    returnval = "Bucket does not exist"
                Else

                    Dim uploadRequest = New TransferUtilityUploadRequest

                    'End With

                    With uploadRequest
                        .BucketName = "novareportfiles"
                        .CannedACL = S3CannedACL.PublicRead
                        '.FilePath = "D:\output.pdf"
                        .InputStream = fs
                        .Key = "A.pdf"
                    End With

                    Dim fileTransferUtility = New TransferUtility(s3Client)
                    fileTransferUtility.Upload(uploadRequest)

                End If

            Catch ex As AmazonS3Exception

                returnval = ex.Message
                MsgBox(ex.Message & ". Update Fails")

            End Try

        Catch ex As Exception

            returnval = ex.Message
            MsgBox(ex.Message & ". Update Fails")

        End Try

        'Return returnval


    End Sub

End Module
