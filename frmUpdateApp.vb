
Imports Amazon.S3
Imports Amazon.S3.Model
Imports Amazon.Runtime
Imports Amazon
Imports Amazon.S3.Util
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Reflection
Public Class frmUpdateApp

    Const AWS_ACCESS_KEY As String = "AKIA3HI2CJWF3PFTLBSW"
    Const AWS_SECRET_KEY As String = "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"

    Public Sub New()



        Try

            's3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, Region.)
            'Dim clientConfig As New AmazonS3Config
            'ClientConfig.RegionEndpoint = RegionEndpoint.APSouth1

            s3Client = New AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.APSouth1)

        Catch ex As Exception

        End Try

        ' This call is required by the designer.

        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Property s3Client As IAmazonS3

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click



    End Sub

    Public Function DownloadFile(bucketName As String, folderName As String) As String

        If Not Directory.Exists(Application.StartupPath & "\downloader") Then
            Directory.CreateDirectory(Application.StartupPath & "\downloader")
        End If

        Dim target As String = Path.GetTempPath()
        Dim returnval As String = ""

        folderName = folderName.Replace("\", "/")

        Try

            Try
                If Not AmazonS3Util.DoesS3BucketExist(s3Client, bucketName) Then
                    returnval = "Bucket does not exist"
                Else
                    Dim request As ListObjectsRequest = New ListObjectsRequest() With {.BucketName = bucketName}
                    Do
                        Dim response As ListObjectsResponse = s3Client.ListObjects(request)
                        For i As Integer = 1 To response.S3Objects.Count - 1
                            Dim entry As S3Object = response.S3Objects(i)
                            'If Replace(entry.Key, folderName & "/", "") = filename Then
                            Dim objRequest As GetObjectRequest = New GetObjectRequest() With {.BucketName = bucketName, .Key = entry.Key}
                            Dim objResponse As GetObjectResponse = s3Client.GetObject(objRequest)

                            'objResponse.WriteResponseStreamToFile("d:\" & FileName)

                            If Not File.Exists(Application.StartupPath & "\downloader\" & entry.Key.Replace(folderName & "/", "")) And entry.Key.Contains(folderName & "\") Then
                                objResponse.WriteResponseStreamToFile(Application.StartupPath & "\downloader\" & entry.Key.Replace(folderName & "/", ""))
                            End If


                        Next

                        Shell(Application.StartupPath & "\downloader\DownloadNApp.exe")
                        Me.Close()
                        Application.Exit()

                        If (response.IsTruncated) Then
                            request.Marker = response.NextMarker
                        Else
                            request = Nothing
                        End If
                    Loop Until IsNothing(request)



                End If

            Catch ex As AmazonS3Exception

                returnval = ex.Message

            End Try
        Catch ex As Exception

            returnval = ex.Message
        End Try
        Return returnval

    End Function

    Private Sub frmUpdateApp_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
