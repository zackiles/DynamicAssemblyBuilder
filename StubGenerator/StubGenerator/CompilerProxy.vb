Imports DynamicFramework.Compiler
Public Class CompilerProxy
    Public Shared InternalManagedResourceCollection As New Dictionary(Of String, String)
    Public Shared InternalUnmanagedResourceList As New List(Of String)
    Public Shared InternalIconLocation As String
    Public Function StartProxy(Optional ByVal runAfterCompile As Boolean = False) As Boolean
        Dim _seriveProvider As New BackingStoreProvider
        Dim _convertToRE As New List(Of ResourceEntity)
        Dim _re As New ResourceEntity
        For Each res In InternalManagedResourceCollection
            _re.Location = res.Key
            _re.ResourceStoringType = res.Value
            _convertToRE.Add(_re)
        Next
        _assemblyStore.ManagedResourceCollection = _convertToRE
        _assemblyStore.UnManagedResourceList = InternalUnmanagedResourceList
        'BackingStoreProvider.OutputLocation = OutputLocation
        BackingStoreProvider.IconLocation = InternalIconLocation
        BackingStoreProvider.LoadStoreAndCompileFromSource(AssemblyStore, StubSourceCode, runAfterCompile)
        Return True
    End Function
    Private _assemblyStore As AssemblyBackingStore
    Public Property AssemblyStore As AssemblyBackingStore
        Get
            Return _assemblyStore
        End Get
        Set(ByVal value As AssemblyBackingStore)
            _assemblyStore = value
        End Set
    End Property
    Private _OutputLocation As String
    Public Property OutputLocation As String
        Get
            Return _OutputLocation
        End Get
        Set(ByVal value As String)
            _OutputLocation = value
        End Set
    End Property
    Private _stubSourceCode As String
    Public Property StubSourceCode As String
        Get
            Return _stubSourceCode
        End Get
        Set(ByVal value As String)
            _stubSourceCode = value
        End Set
    End Property
    Private _resourceFileCollection As String()
    Public Property ResourceFileCollection As String()
        Get
            Return _resourceFileCollection
        End Get
        Set(ByVal value As String())
            _resourceFileCollection = value
        End Set
    End Property
End Class
