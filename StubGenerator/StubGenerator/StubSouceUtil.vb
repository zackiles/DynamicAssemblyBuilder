Public Class StubSouceUtil
    Public Shared rand As New Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))
    Private _declarationToReplaceList As HashSet(Of String)
    Public Shared Sub Configure(ByRef stubSource As String)
        Dim stubSourceUtil As New StubSouceUtil
        stubSourceUtil.RandomizeManager(stubSource)
    End Sub
    Private Sub RandomizeManager(ByRef stubSource As String)
        _declarationToReplaceList = Nothing
        _declarationToReplaceList = New HashSet(Of String)
        '   The globals are variables that you've pre defined by enclosing the variable in tags. See Structure StubGlobal.
        RandomizeStubGlobals(stubSource)
        '   Iterate the stub source lines for declarations defined in VbDeclaration, and adds them to a hash list.
        For Each declaration In GetType(VbDeclaration).GetFields
            DeclarationsToList(stubSource, declaration.GetValue(declaration))
        Next
        '   Iterate the declartionToReplaceList; randomize all the names, and replace the stub source with them.
        RandomizeDeclarationList(stubSource)
    End Sub
    Public Sub DeclarationsToList(ByRef stubSource As String, ByVal vbDeclaration As String)
        Dim indexList As New List(Of Integer)()
        Dim indexAdjustToDeclaration As Integer = (vbDeclaration.Length + 1)
        '   Add's all start indices for substrings that match a vbDeclaration.
        For i As Integer = 0 To stubSource.Length - 1
            If Not i >= stubSource.Length - vbDeclaration.Length Then
                If stubSource.Substring(i, indexAdjustToDeclaration).Equals(vbDeclaration + " ") Then
                    indexList.Add(i)
                End If

            End If
        Next
        '   Uses the indices from above and adds the substring for each variable into a list.
        For Each i As Integer In indexList
            If vbDeclaration.Equals("Class") Then
                '   Cleanup for class declarations.
                vbDeclaration = stubSource.Substring((i + indexAdjustToDeclaration), (stubSource.IndexOf(" ", (i + indexAdjustToDeclaration)) - (i + indexAdjustToDeclaration)))
                indexAdjustToDeclaration = vbDeclaration.IndexOfAny(New Char() {Chr(10), Chr(13), Environment.NewLine})
                If Not indexAdjustToDeclaration.Equals(-1) Then
                    Dim removeLocation As Integer = (vbDeclaration.Length - indexAdjustToDeclaration)
                    vbDeclaration = vbDeclaration.Remove(indexAdjustToDeclaration, removeLocation)
                End If
            Else
                vbDeclaration = stubSource.Substring((i + indexAdjustToDeclaration), (stubSource.IndexOf(" ", (i + indexAdjustToDeclaration)) - (i + indexAdjustToDeclaration)))
            End If
            If vbDeclaration.Contains("(") Then
                vbDeclaration = vbDeclaration.Remove(vbDeclaration.IndexOf("("), (vbDeclaration.Length - vbDeclaration.IndexOf("(")))
            End If
            '   Change all declarations but the static main, and do final verification.
            If Not vbDeclaration.Equals("Main") Then
                If Not _declarationToReplaceList.Contains(vbDeclaration) Then
                    If Not vbDeclaration.Length.Equals(0) Then
                        _declarationToReplaceList.Add(vbDeclaration.TrimEnd)
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub RandomizeDeclarationList(ByRef stubSource As String)
        For i As Integer = 0 To _declarationToReplaceList.Count - 1
            stubSource = stubSource.Replace(_declarationToReplaceList(i), GenerateRandomString(rand.[Next](5, 9), rand))
        Next
    End Sub
    Private Sub RandomizeStubGlobals(ByRef stubSource As String)
      
            stubSource = stubSource.Replace(StubGlobal._rootNamespace, GenerateRandomString(rand.[Next](5, 9), rand))

    End Sub
    Public Function GenerateRandomString(ByVal size As Integer, ByVal rand As Random) As String
        '   Usage  >>  GenerateRandomString(rand.[Next](5, 9), rand))
        Dim ret As New System.Text.StringBuilder()
        For i As Integer = 0 To size - 1
            Dim type As Integer = rand.[Next](0, 2)
            Select Case type
                Case 0
                    ret.Append(Convert.ToChar(rand.[Next](&H61, &H7A)))
                    Exit Select
                Case 1
                    ret.Append(Convert.ToChar(rand.[Next](&H41, &H5A)))
                    Exit Select
                    'Case 2
                    '    '0 - 9
                    '    ret.Append(Convert.ToChar(rand.[Next](&H30, &H39)))
                    '    Exit Select
            End Select
        Next
        Return ret.ToString()
    End Function
    Structure StubGlobal
        Public Const _rootNamespace As String = "<$ROOTNAMESPACE$>"
    End Structure
    Structure VbDeclaration
        Public Const _dim As String = "Dim"
        Public Const _function As String = "Function"
        Public Const _sub As String = "Sub"
        Public Const _class As String = "Class"
        Public Const _property As String = "Property"
        Public Const _stucture As String = "Structure"
    End Structure
End Class
