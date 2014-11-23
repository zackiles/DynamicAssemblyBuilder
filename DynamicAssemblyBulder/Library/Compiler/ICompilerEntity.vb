Public Interface ICompilerEntity
    Property Name As String
    Property Location As String
    Property RootNamespace As String
    ReadOnly Property GetEntityAsObject As Object
    WriteOnly Property SetEntityAsObject As Object
End Interface
