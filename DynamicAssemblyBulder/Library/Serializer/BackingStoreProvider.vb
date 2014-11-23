Imports DynamicFramework.Compiler
Imports DynamicFramework.Application
Namespace Compiler
    Public Class BackingStoreProvider
        Private Shared _assemblyStore As AssemblyDefinition
        Private Shared _sourceCodeFileCollection As List(Of String)
        Private Shared _outputLocation As String
        '   Of Resource Location + Resource Storing Type
        Private Shared _resourceList As List(Of ResourceEntity)
        Public Shared IconLocation As String = Nothing
        Public Shared Sub CreateStoreXml(Optional ByVal fileName As String = Nothing)

            Dim serializer As New Serializer.Xml
            Dim c1 As New AssemblyBackingStore
            serializer.Serialize(c1)

        End Sub
        Public Shared Sub CreateStoreBinary(Optional ByVal fileName As String = Nothing)
            Dim serializer As New Serializer.Binary
            Dim c1 As New AssemblyBackingStore
            serializer.Serialize(c1)

        End Sub

        Public Shared Sub LoadStoreAndCompileFromSource(ByRef _config As AssemblyBackingStore, _
                                                        Optional ByVal sourceCode As String = Nothing, _
                                                        Optional ByVal runAfterCompile As Boolean = False)
            _assemblyStore = New AssemblyDefinition
            Dim sourceFile As New SourceCodeEntity
            '   Property constructs from serilization
            sourceFile.SetEntityAsObject = sourceCode
            _assemblyStore.AddSourceCode(sourceFile)
            LoadStore(_config, runAfterCompile)
        End Sub
        Public Shared Sub LoadStoreAndCompileFromFile(ByRef _config As AssemblyBackingStore, _
                                                      Optional ByVal fileName As String = Nothing, _
                                                      Optional ByVal runAfterCompile As Boolean = False)
            _assemblyStore = New AssemblyDefinition
            Dim sourceFile As New SourceCodeEntity
            '   Property constructs from serilization
            If fileName.Equals(Nothing) Then
                sourceFile.Location = Settings.GetSourceCodeLocation(_config.StubName)
            Else
                sourceFile.Location = fileName
            End If
            LoadStore(_config, runAfterCompile)
        End Sub

        Private Overloads Shared Sub LoadStore(ByRef config As AssemblyBackingStore, ByVal runafterCompile As Boolean)

            '   *Method constructs from serilization
            Select Case config.SourceTarget
                Case ".exe"
                    _assemblyStore.SetSourceTarget(AssemblyDefinition.SourceTarget.EXE)
                Case ".dll"
                    _assemblyStore.SetSourceTarget(AssemblyDefinition.SourceTarget.DLL)
                Case ".netmodule"
                    _assemblyStore.SetSourceTarget(AssemblyDefinition.SourceTarget.NETMODULE)
            End Select
            _assemblyStore.Name = config.RootNamespace
            _assemblyStore.SetClrVersion(config.RuntimeVersion)
            _assemblyStore.SetFileAlignment(config.FileAlignment)
            _assemblyStore.SetImageBase(config.ImageBase)
            _assemblyStore.SetSizeOfStackReserve(config.StackReserve)
            _assemblyStore.ShouldFoldMethodBodies = config.FoldMethodBodies
            _assemblyStore.IncludeDebugInfo = config.IncludeDebugInfo
            _assemblyStore.Optimized = config.Optimized
            _assemblyStore.StripRelocationTable = config.StripRelocationTable
            _assemblyStore.ShowConsoleWindow = config.ShowConsoleWindow
            _assemblyStore.NoLogo = config.NoLogo
            _assemblyStore.StrongSigningOn = config.StrongSigned
            _assemblyStore.ReferencedAssemblies = config.ReferencedAssemblies
            If Not IsNothing(IconLocation) Then
                _assemblyStore.Icon = IconLocation
            End If

            '   Creates all unmanaged resources in assembly.
            If Not IsNothing(config.UnManagedResourceList) Then
                _resourceList = New List(Of ResourceEntity)
                Dim resEntity As ResourceEntity
                For Each file As String In config.UnManagedResourceList
                    resEntity = New ResourceEntity
                    resEntity.Location = file
                    _resourceList.Add(resEntity)
                Next
                _assemblyStore.UnmanagedResourceList = _resourceList
            End If
            '  Creates a managed .res file with multiple packaged resources inside.
            If Not config.ManagedResourceCollection.Count <= 0 Then
                Dim listOfManRes As New List(Of ManagedResourceEntity)
                Dim managedResourceProvider As ManagedResourceProvider = New ManagedResourceProvider
                Dim managedResourceEntity As ManagedResourceEntity = New ManagedResourceEntity
                managedResourceEntity.CreateStronglyTypedClass = True
                managedResourceEntity.ResourceInClassList = config.ManagedResourceCollection
                managedResourceEntity.RootNamespace = config.RootNamespace
                managedResourceProvider.CreateManagedResourcePackage(managedResourceEntity)
                listOfManRes.Add(managedResourceEntity)
                _assemblyStore.ManagedResourceList = listOfManRes
            End If

            ' Add source codes to compile
            '
            If Not IsNothing(_sourceCodeFileCollection) Then
                Dim sourceEntityEntry As SourceCodeEntity
                For Each sourceLocationInCollection As String In _sourceCodeFileCollection
                    sourceEntityEntry = New SourceCodeEntity
                    sourceEntityEntry.Location = sourceLocationInCollection
                    _assemblyStore.AddSourceCode(sourceEntityEntry)
                Next
            End If

            '    Start Compile.
            Dim codeDomCompailer As New Compiler.CodeDomAgent
            _assemblyStore.Location = OutputLocation
            codeDomCompailer.GenerateAssembly(_assemblyStore, runafterCompile)

            ' Optional Debug Mode
            Logger.PrintAllEntriesToDebug()
        End Sub
        Public Shared Property OutputLocation As String
            Get
                Return _outputLocation
            End Get
            Set(ByVal value As String)
                _outputLocation = value
            End Set
        End Property
    End Class
End Namespace