Public MustInherit Class EntityBase
    Implements ICompilerEntity
    Private _entityObjectValue As Object
    Private _name As String
    Private _rootNamespace As String
    Private _location As String
    Public Overridable ReadOnly Property GetEntityAsObject As Object Implements ICompilerEntity.GetEntityAsObject
        Get
            If IsNothing(_entityObjectValue) Then
                If Not IsNothing(Location) Then
                    If IO.File.Exists(Location) Then
                        Select Case IO.Path.GetExtension(Location)
                            Case ".txt", ".xml", ".rtf", ".html", ".doc", ".vb", ".cs"
                                _entityObjectValue = IO.File.ReadAllText(Location)
                            Case Else
                                _entityObjectValue = IO.File.ReadAllBytes(Location)
                        End Select
                    End If
                End If
            End If
            Return _entityObjectValue
        End Get
    End Property
    Public Overridable WriteOnly Property SetEntityAsObject As Object Implements ICompilerEntity.SetEntityAsObject
        Set(ByVal value As Object)
            _entityObjectValue = value
        End Set
    End Property
    Public Overridable Property RootNamespace As String Implements ICompilerEntity.RootNamespace
        Get
            If IsNothing(_rootNamespace) Then
                Return Name
            Else
                Return _rootNamespace
            End If
        End Get
        Set(ByVal value As String)
            _rootNamespace = value
        End Set
    End Property
    Public Overridable Property Name As String Implements ICompilerEntity.Name
        Get
            If IsNothing(_name) Then
                If Not IsNothing(Location) Then
                    Try
                        If IO.File.Exists(Location) Then
                            _name = IO.Path.GetFileNameWithoutExtension(Location)
                        End If
                    Catch
                    End Try
                End If
            End If
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Public Overridable Property Location As String Implements ICompilerEntity.Location
        Get
            Return _location
        End Get
        Set(ByVal value As String)
            _location = value
        End Set
    End Property
End Class
