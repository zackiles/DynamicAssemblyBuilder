Imports DynamicFramework.Compiler
Namespace Application
    Public Class ConfigurationWizard
        Private _managedResourceFileCollection As List(Of String)
        Private _embeddedResourceFileCollection As List(Of String)
        Private _sourceCodeFileCollection As List(Of String)
        Private _outputLocation As String
        Private _compilerAgent As Type
        Private _assemblyDefinition As AssemblyDefinition
        Sub New()
            _assemblyDefinition = New AssemblyDefinition
        End Sub
        Enum CompilerAgentType
            CodeDomAgent
            IlAsmAgent
        End Enum
        Public Sub Start()
            '    Just simple compile plementation for fas source -> PE
            '   Might be good to use this serialized for quick unit testing source code generators
            Dim managedResourceProvider As New Compiler.ManagedResourceProvider
            Dim managedResourceEntity As New Compiler.ManagedResourceEntity
            Dim resourceEntity As New Compiler.ResourceEntity
            Dim sourceCodeEntity As New SourceCodeEntity
            Dim codeDomCompailer As New Compiler.CodeDomAgent
            '   Set the assemblyname
            _assemblyDefinition.Name = IO.Path.GetFileNameWithoutExtension(OutputLocation)

            '   Add/Check for managed resource files
            If Not IsNothing(ManagedResourceFileCollection) Then
                Dim listOfManRes As New List(Of ManagedResourceEntity)
                For Each managedResLocationInCollection As String In _managedResourceFileCollection
                    managedResourceEntity = New ManagedResourceEntity
                    managedResourceEntity.Location = managedResLocationInCollection
                    managedResourceEntity.CreateStronglyTypedClass = True
                    managedResourceProvider.CreateManagedResourcePackage(managedResourceEntity)
                    listOfManRes.Add(managedResourceEntity)
                Next
                _assemblyDefinition.ManagedResourceList = listOfManRes
            End If

            Dim sourceEntityEntry As SourceCodeEntity
            For Each sourceLocationInCollection As String In _sourceCodeFileCollection
                sourceEntityEntry = New SourceCodeEntity
                sourceEntityEntry.Location = sourceLocationInCollection
                _assemblyDefinition.AddSourceCode(sourceEntityEntry)
            Next


            codeDomCompailer.GenerateAssembly(_assemblyDefinition)


        End Sub
        Public Property CompilerAgent As CompilerAgentType
            Get
                If Not IsNothing(_compilerAgent) Then
                    Select Case _compilerAgent.GetType
                        Case GetType(CodeDomAgent)
                            Return CompilerAgentType.CodeDomAgent
                        Case GetType(IlasmAgent)
                            Return CompilerAgentType.IlAsmAgent
                    End Select
                End If
                Return Nothing
            End Get
            Set(ByVal value As CompilerAgentType)
                Select Case value.ToString
                    Case "CodeDomAgent"
                        _compilerAgent = GetType(CodeDomAgent)
                    Case "IlAsmAgent"
                        _compilerAgent = GetType(IlasmAgent)
                End Select
            End Set
        End Property
        Private Function StringArrayToList(ByVal inArray As String()) As List(Of String)
            If Not IsNothing(inArray) Then
                Dim listHolder As New List(Of String)
                For i As Integer = 0 To inArray.Count - 1
                    listHolder.Add(inArray(i))
                Next
                Return listHolder
            End If
            Return Nothing
        End Function
        Private Function ListToStringArray(ByVal inList As List(Of String)) As String()
            If Not IsNothing(inList) Then
                Dim arrayHolder As String() = New String(inList.Count - 1) {}
                For i As Integer = 0 To inList.Count - 1
                    arrayHolder(i) = inList(i)
                Next
                Return arrayHolder
            End If
            Return Nothing
        End Function
        Public Property OutputLocation As String
            Get
                Return _outputLocation
            End Get
            Set(ByVal value As String)
                _outputLocation = value
            End Set
        End Property

        Public Property SourceCodeFileCollection As String()
            Get
                Return ListToStringArray(_sourceCodeFileCollection)
            End Get
            Set(ByVal value As String())
                _sourceCodeFileCollection = StringArrayToList(value)
            End Set
        End Property
        Public Property EmbeddedResourceFileCollection As String()
            Get
                Return ListToStringArray(_embeddedResourceFileCollection)
            End Get
            Set(ByVal value As String())
                _embeddedResourceFileCollection = StringArrayToList(value)
            End Set
        End Property

        Public Property ManagedResourceFileCollection As String()
            Get
                Return ListToStringArray(_managedResourceFileCollection)
            End Get
            Set(ByVal value As String())
                _managedResourceFileCollection = StringArrayToList(value)
            End Set
        End Property


    End Class
End Namespace