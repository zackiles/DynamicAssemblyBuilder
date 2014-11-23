Imports System.Reflection
Namespace Application
    Public Class ResourceCache
        Private Shared _managedResourceCache As Collections.Generic.Dictionary(Of String, Reflection.PropertyInfo)
        Private Shared _nativedResourceCache As List(Of String)
        Private Shared _isCreated As Boolean = False
        Private Shared _currentAssembly As Assembly = Assembly.GetExecutingAssembly
        Public Shared Function GetResourceFileAsString(ByVal name As String, Optional ByVal exactMatch As Boolean = False) As String
            Try
                Dim sReader As IO.StreamReader = New IO.StreamReader(DirectCast(GrabResource(name, exactMatch), IO.Stream))
                Return sReader.ReadToEnd
            Catch
                Return Nothing
            End Try
        End Function
        Public Shared Function GetResourceFileAsBytes(ByVal name As String, Optional ByVal exactMatch As Boolean = False) As Byte()
            Try
                Dim returnValue As Object = GrabResource(name, exactMatch)
                Select Case returnValue.GetType.Name
                    Case "Stream", "UnmanagedMemoryStream"
                        returnValue = DirectCast(returnValue, IO.UnmanagedMemoryStream)
                        Dim buffer As Byte() = New Byte(returnValue.Length) {}
                        returnValue.Read(buffer, 0, buffer.Length)
                        Return buffer



                End Select
            Catch
                Return Nothing
            End Try
        End Function
        Private Shared Function GrabResource(ByVal name As String, ByVal exactMatch As Boolean) As Object
            If Not _isCreated Then
                CreateCatche()
                _isCreated = True
            End If

            Dim buffer As Object = Nothing
            For Each s In _nativedResourceCache
                If ResourceNameMatch(s, name, exactMatch) Then
                    Return _currentAssembly.GetManifestResourceStream(s)
                End If
            Next

            Dim resourceClassGetProperty As PropertyInfo
            For Each s In _managedResourceCache.Keys
                If ResourceNameMatch(s, name, exactMatch) Then
                    resourceClassGetProperty = _managedResourceCache(s)
                    buffer = resourceClassGetProperty.GetValue(New Object() {GetType(My.Resources.Resources)}, Nothing)
                    Return buffer
                End If
            Next
            Return Nothing
        End Function
        Private Shared Function ResourceNameMatch(ByVal source As String, ByVal toCompare As String, ByVal exactMatch As Boolean) As Boolean
            source = source.ToLower
            toCompare = toCompare.ToLower
            If exactMatch Then
                If source.Equals(toCompare) Then
                    Return True
                End If
            Else
                If source.Contains(toCompare) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Private Shared Sub CreateCatche()
            _managedResourceCache = New Collections.Generic.Dictionary(Of String, Reflection.PropertyInfo)
            _nativedResourceCache = New List(Of String)
            Try
                Dim resourceClass As Type = _currentAssembly.GetType( _
                    String.Format("{0}.My.Resources.Resources", _
                                  _currentAssembly.GetName.Name), True)
                Dim internalType1 As Type
                Dim internalType2 As Type
                For Each pi As Reflection.PropertyInfo In resourceClass.GetProperties(8 Or 32 Or 4)
                    internalType1 = GetType(Globalization.CultureInfo)
                    internalType2 = GetType(Resources.ResourceManager)
                    If Not pi.PropertyType.Equals(internalType1) And Not pi.PropertyType.Equals(internalType2) Then
                        _managedResourceCache.Add(pi.Name, pi)
                    End If
                Next
            Catch
            End Try
            For Each res As String In _currentAssembly.GetManifestResourceNames()
                _nativedResourceCache.Add(res)
            Next
        End Sub
    End Class
End Namespace