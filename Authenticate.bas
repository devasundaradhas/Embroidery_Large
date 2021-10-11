Module Authenticate

    Public Function AuthenticationCode(ByVal cmpname As String) As String

        Randomize()

        Dim RotationString As String = ""
        Dim RotationString1 As String = System.Math.Round(Rnd() * 1000, 0)

        For j As Int16 = 1 To Len(cmpname)
            If Len(Trim(RotationString)) = 0 Then
                RotationString = System.Math.Round(Rnd() * 10, 0).ToString
            Else

                RotationString = RotationString + System.Math.Round(Rnd() * 9, 0).ToString
            End If
        Next


        Dim len1, len2 As String
        Dim ACode1T As String = ""
        Dim ACode2T As String = ""
        len1 = Len(cmpname) \ 2
        len2 = Len(cmpname) - len1
        Dim CaseStr As String = ""

        For I = 1 To Len(cmpname)
            If Asc(Mid(cmpname, I, 1)) < 65 Or Asc(Mid(cmpname, I, 1)) > 90 Then
                CaseStr = CaseStr + "0"
            Else
                CaseStr = CaseStr + "1"
            End If
        Next

        cmpname = UCase(cmpname)

        Dim Acode1 As String = ""
        Dim Acode2 As String = ""

        For i As Integer = len1 To 1 Step -1

            If Len(CStr(Asc(Mid(cmpname, i, 1)))) = 1 Then
                ACode1T = "90"
            ElseIf Len(CStr(Asc(Mid(cmpname, i, 1)))) = 2 Then
                ACode1T = "9"
            End If

            ACode1T = ACode1T & CStr(Asc(Mid(cmpname, i, 1)))
            ACode1T = CStr(1000 - Val(ACode1T))

            If Val(ACode1T) < 10 Then
                ACode1T = "00" + ACode1T
            ElseIf Val(ACode1T) < 100 Then
                ACode1T = "0" + ACode1T
            End If


            Acode1 = Acode1 & ACode1T

        Next


        For i = len1 + 1 To Len(cmpname)

            If Len(CStr(Asc(Mid(cmpname, i, 1)))) = 1 Then
                ACode2T = "90"
            ElseIf Len(CStr(Asc(Mid(cmpname, i, 1)))) = 2 Then
                ACode2T = "9"
            End If

            ACode2T = ACode2T & Asc(Mid(cmpname, i, 1))

            ACode2T = CStr(1000 - Val(ACode2T))

            If Val(ACode2T) < 10 Then
                ACode2T = "00" + ACode2T
            ElseIf Val(ACode2T) < 100 Then
                ACode2T = "0" + ACode2T
            End If

            Acode2 = Acode2 & ACode2T

        Next

        AuthenticationCode = Acode1 & Acode2

        Dim RevCode As String = ""

        For i = Len(AuthenticationCode) To 1 Step -1

            RevCode = RevCode + Mid(AuthenticationCode, i, 1)

        Next


        Dim RevCode1(Len(cmpname) - 1) As String
        Dim T As String = ""

        For i = 0 To Len(cmpname) - 1

            RevCode1(i) = Mid(RevCode, (i * 3) + 1, 3)

            Dim RevCodeSubStr3(2) As String

            If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 0 Then

                RevCodeSubStr3(2) = Mid(RevCode1(i), 3, 1)
                RevCodeSubStr3(1) = Mid(RevCode1(i), 2, 1)
                RevCodeSubStr3(0) = Mid(RevCode1(i), 1, 1)

            End If
            If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 1 Then

                RevCodeSubStr3(2) = Mid(RevCode1(i), 2, 1)
                RevCodeSubStr3(1) = Mid(RevCode1(i), 1, 1)
                RevCodeSubStr3(0) = Mid(RevCode1(i), 3, 1)

            End If

            If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 2 Then

                RevCodeSubStr3(2) = Mid(RevCode1(i), 1, 1)
                RevCodeSubStr3(1) = Mid(RevCode1(i), 3, 1)
                RevCodeSubStr3(0) = Mid(RevCode1(i), 2, 1)

            End If

            RevCode1(i) = Join(RevCodeSubStr3, "")

        Next

       
        If Val(RotationString1) Mod Len(cmpname) <> 0 Then

            For i = 1 To Val(RotationString1) Mod Len(cmpname)

                T = RevCode1(Len(cmpname) - 1)

                For j = Len(cmpname) - 2 To 0 Step -1

                    RevCode1(j + 1) = RevCode1(j)

                Next

                RevCode1(0) = T
            Next

        End If

        RevCode = Join(RevCode1, "")

       
        AuthenticationCode = RevCode + "," + RotationString + "," + RotationString1 + "," + CaseStr

       

    End Function

    Public Function RevertAuthenticationCode(ByVal Code As String) As String

        
        If Len(Code) <= 3 Then
            RevertAuthenticationCode = ""
            Exit Function
        End If


        If UBound(Split(Code, ",")) <> 3 Then
            RevertAuthenticationCode = ""
            Exit Function
        End If

        Dim RotationString As String = ""
        Dim RotationString1 As Int16 = 0
        Dim CaseString As String = ""

        RotationString = Split(Code, ",")(1)
        RotationString1 = Val(Split(Code, ",")(2))
        CaseString = Split(Code, ",")(3)
        Code = Split(Code, ",")(0)


        ' rotate the entrire string by chunks of 3

        Dim Code1(Len(RotationString) - 1) As String
        Dim T As String = ""

        '  seggregate the code into chunks of 3

        For I As Int16 = 0 To Len(RotationString) - 1
            Code1(I) = Mid(Code, I * 3 + 1, 3)
        Next

        ' reverse the overall rotation done in encrytion

        If RotationString1 Mod Len(RotationString) <> 0 Then

            For i = 1 To RotationString1 Mod Len(RotationString)

                T = Code1(0)

                For j = 1 To UBound(Code1)

                    Code1(j - 1) = Code1(j)

                Next

                Code1(UBound(Code1)) = T

            Next

        End If


        'MsgBox(Join(Code1, ""))
        '------------------------------------------------

        'reverse rotation within the chunks

        Dim Code2(2) As String

        For i = 0 To Len(RotationString) - 1

            'If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 0 Then  no action is to be taken

            If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 1 Then
                Code2(0) = Mid(Code1(i), 2, 1)
                Code2(1) = Mid(Code1(i), 3, 1)
                Code2(2) = Mid(Code1(i), 1, 1)

                Code1(i) = Join(Code2, "")

            End If

            If Val(Mid(RotationString, i + 1, 1)) Mod 3 = 2 Then
                Code2(0) = Mid(Code1(i), 3, 1)
                Code2(1) = Mid(Code1(i), 1, 1)
                Code2(2) = Mid(Code1(i), 2, 1)

                Code1(i) = Join(Code2, "")

            End If

        Next

        Code = Join(Code1, "")

        'MsgBox(Code)

        '-------------------------------------------------


        Dim RevCode As String = ""

        For I = Len(Code) To 1 Step -1

            RevCode = RevCode + Mid(Code, I, 1)

        Next

        Code = RevCode

        Dim tlen As Integer
        Dim len1 As Integer
        Dim len2 As Integer
        Dim cmpname As String = ""

        Dim TMPSTR As String

        tlen = Len(Code)

        len1 = tlen / 2
        len1 = len1 - len1 Mod 3

        len2 = tlen - len1

        Dim K As Integer = len1
        Dim CurrChar As Int16 = 1

        Do While K > 0

            TMPSTR = CStr(Val(Mid(Code, K - 2, 3)))
            TMPSTR = CStr(1000 - Val(TMPSTR))


            If Left(TMPSTR, 2) = "90" Then
                If Mid(CaseString, CurrChar, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(Mid(TMPSTR, 3, 1))))
                Else
                    cmpname = cmpname + Chr(Val(Mid(TMPSTR, 3, 1)))
                End If
            ElseIf Left(TMPSTR, 1) = "9" Then
                If Mid(CaseString, CurrChar, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(Mid(TMPSTR, 2, 2))))
                Else
                    cmpname = cmpname + Chr(Val(Mid(TMPSTR, 2, 2)))
                End If
            Else
                If Mid(CaseString, CurrChar, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(TMPSTR)))
                Else
                    cmpname = cmpname + Chr(Val(TMPSTR))
                End If
            End If

            K = K - 3
            CurrChar = CurrChar + 1

        Loop

        K = len1 + 1

        Do While K < tlen

            TMPSTR = CStr(Val(Mid(Code, K, 3)))
            TMPSTR = CStr(1000 - Val(TMPSTR))

            If Left(TMPSTR, 2) = "90" Then
                If Mid(CaseString, (K + 2) / 3, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(Mid(TMPSTR, 3, 1))))
                Else
                    cmpname = cmpname + Chr(Val(Mid(TMPSTR, 3, 1)))
                End If
            ElseIf Left(TMPSTR, 1) = "9" Then
                If Mid(CaseString, (K + 2) / 3, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(Mid(TMPSTR, 2, 2))))
                Else
                    cmpname = cmpname + Chr(Val(Mid(TMPSTR, 2, 2)))
                End If
            Else
                If Mid(CaseString, (K + 2) / 3, 1) = "0" Then
                    cmpname = cmpname + LCase(Chr(Val(TMPSTR)))
                Else
                    cmpname = cmpname + Chr(Val(TMPSTR))
                End If
            End If

            K = K + 3

        Loop

        
        RevertAuthenticationCode = cmpname

    End Function

End Module
