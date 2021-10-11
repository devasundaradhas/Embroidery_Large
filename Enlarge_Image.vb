Public Class Enlarge_Image
    Implements Interface_MDIActions

    Dim vPicImg As Image

    Public Sub New(ByVal PicImage As Image)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        vPicImg = PicImage
    End Sub

    Private Sub Enlarge_Image_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PictureBox1.BackgroundImage = vPicImg
    End Sub

    Private Sub Enlarge_Image_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Dispose()
        End If
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Dispose()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Dim myPrintDocument1 As System.Drawing.Printing.PrintDocument = New System.Drawing.Printing.PrintDocument
        myPrintDocument1.DefaultPageSettings.PaperSize = New System.Drawing.Printing.PaperSize("A4", 827, 1170)

        Dim myPrinDialog1 As PrintDialog = New PrintDialog
        AddHandler myPrintDocument1.PrintPage, AddressOf Me.myPrintDocument1_PrintPage

        myPrinDialog1.Document = myPrintDocument1
        If (myPrinDialog1.ShowDialog = Windows.Forms.DialogResult.OK) Then
            myPrintDocument1.Print()
        End If

    End Sub

    Private Sub myPrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim vImg As Image = PictureBox1.BackgroundImage

        Dim newWidth As Decimal = vImg.Width * 100 / vImg.HorizontalResolution  '700 ' 
        Dim newHeight As Decimal = vImg.Height * 100 / vImg.VerticalResolution   '800 ' 

        Dim widthFactor As Decimal = newWidth / e.MarginBounds.Width
        Dim heightFactor As Decimal = newHeight / e.MarginBounds.Height

        If (widthFactor > 1 Or heightFactor > 1) Then
            If (widthFactor > heightFactor) Then
                newWidth = newWidth / widthFactor
                newHeight = newHeight / widthFactor
            Else
                newWidth = newWidth / heightFactor
                newHeight = newHeight / heightFactor
            End If
        End If

        e.Graphics.DrawImage(vImg, 50, 50, Int(newWidth), Int(newHeight))

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '---
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '---
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '---
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '---
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '---
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '---
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '---
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '---
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '---
    End Sub

End Class