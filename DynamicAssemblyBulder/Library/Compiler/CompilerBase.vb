Imports DynamicFramework.Application

Namespace Compiler
    Public MustInherit Class CompilerBase
        Public _frameworkVersion As Collections.Generic.Dictionary(Of String, String)
        Public _tfc As System.CodeDom.Compiler.TempFileCollection
        Public _sourceFiles As List(Of SourceCodeEntity)
        Public _sourceArray As String()
        Public _compilerArguemnts As String
        Public _compilerVerbosity = False
        MustOverride Sub ConfigurePreCompile(ByRef assemblyDefinition As AssemblyDefinition)
        MustOverride Function ConfigureCompileOnStart(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
        MustOverride Function AddResources(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
        MustOverride Sub BeginCompilation(ByRef assemblyDefinition As AssemblyDefinition)
        MustOverride Sub AddAssemblyReferences(ByRef assemblyDefinition As AssemblyDefinition)
        MustOverride Sub AddStrongNameKeyPair(ByRef assemblyDefinition As AssemblyDefinition)
        Sub CheckForStrongNameSigning(ByRef assemblyDefinition As AssemblyDefinition)
            If assemblyDefinition.StrongSigningOn = True Then
                If IsNothing(assemblyDefinition.StrongSignKeyPairLocation) Then
                    assemblyDefinition.StrongSignKeyPairLocation = StrongNameAgent.GenerateKeyPair(assemblyDefinition)
                    Logger.AddToLog(String.Format("Keypair not selected, Auto genereated new keypair at '{0}'", assemblyDefinition.StrongSignKeyPairLocation))
                End If
                If IO.File.Exists(assemblyDefinition.StrongSignKeyPairLocation) Then
                    AddStrongNameKeyPair(assemblyDefinition)
                Else
                    InternalException.ThrowException(String.Format("Keypair file not found at '{0}'", assemblyDefinition.StrongSignKeyPairLocation), New Object() {Me})
                End If
            End If
        End Sub

        Property CompilerIsVerbose As Boolean
            Get
                Return _compilerVerbosity
            End Get
            Set(ByVal value As Boolean)
                _compilerVerbosity = value
            End Set
        End Property
        Sub Initialize()

        End Sub
        Overloads Sub GenerateAssembly(ByRef configurationWizard As ConfigurationWizard)
            Application.InternalException.ThrowException(Application.InternalMessage.NotImplementedException, New Object() {Me})
        End Sub
        Overloads Sub GenerateAssembly(ByRef assemblyDefinition As AssemblyDefinition, Optional ByVal runAfterCompiled As Boolean = False)

            Select Case assemblyDefinition.GetSourceTarget

                Case ".exe"
                    assemblyDefinition.Location = Application.Settings.GetWorKSpaceFolder + "\" + IO.Path.GetFileNameWithoutExtension(assemblyDefinition.Name) + ".exe"
                Case ".dll"
                    assemblyDefinition.Location = Application.Settings.GetWorKSpaceFolder + "\" + IO.Path.GetFileNameWithoutExtension(assemblyDefinition.Name) + ".dll"
                Case ".netmodule"
                    assemblyDefinition.Location = Application.Settings.GetWorKSpaceFolder + "\" + IO.Path.GetFileNameWithoutExtension(assemblyDefinition.Name) + ".netmodule"
                    Application.InternalException.ThrowException(Application.InternalMessage.NotImplementedException, New Object() {Me})
            End Select
            If IO.File.Exists(assemblyDefinition.Location) Then
                Try
                    IO.File.Delete(assemblyDefinition.Location)
                Catch ex As Exception
                    InternalException.ThrowException(ex)
                End Try
            End If
            Try
                Dim tempSourceArray = New Object(assemblyDefinition.GetSourceCode.Count - 1) {}
                _sourceArray = New String(assemblyDefinition.GetSourceCode.Count - 1) {}
                For i As Integer = 0 To tempSourceArray.Count - 1
                    tempSourceArray(i) = DirectCast(assemblyDefinition.GetSourceCodeFromIndex(i), SourceCodeEntity).GetEntityAsObject
                    Select Case tempSourceArray(i).GetType
                        Case GetType(String)
                            _sourceArray(i) = DirectCast(tempSourceArray(i), String)
                        Case GetType(Byte())
                            'Convert to source string
                            _sourceArray(i) = System.Text.Encoding.UTF8.GetString(tempSourceArray(i))
                        Case GetType(IO.Stream)
                            Dim streamToString As New IO.StreamReader(DirectCast(tempSourceArray(i), IO.Stream))
                            _sourceArray(i) = streamToString.ReadToEnd
                    End Select
                Next
            Catch ex As Exception
                InternalException.ThrowException("There was an error loading the sourcecode, loading return : " + ex.Message, New Object() {Me})
            End Try
            CompileUnit(assemblyDefinition)
            Try
                If runAfterCompiled Then
                    Dim pa As New Application.ShellProcessAgent()
                    pa.ProcessLocation = assemblyDefinition.Location
                    pa.StartShell("")

                End If
            Catch
                Logger.AddToLog("ERROR : File not created.")
            End Try
        End Sub

        Function CompileUnit(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
            Try
                ConfigurePreCompile(assemblyDefinition)
                ConfigureCompileOnStart(assemblyDefinition)
                AddResources(assemblyDefinition)
                AddAssemblyReferences(assemblyDefinition)
                CheckForStrongNameSigning(assemblyDefinition)
                BeginCompilation(assemblyDefinition)
                FinalizeCompilation()
                Return True
            Catch ex As Exception
                InternalException.ThrowException(ex)
                Return False
            End Try
            Return True
        End Function
        Sub FinalizeCompilation()
            CleanUpWorkSpace()
        End Sub

        Sub CleanUpWorkSpace()

            'Application.Logger.AddToLog(String.Format("Attempting cleanup of temp files in  {0}", _tfc.BasePath))
            'If IO.Directory.Exists(_tfc.BasePath) Then
            '    For Each file As IO.FileInfo In New IO.DirectoryInfo(_tfc.BasePath).GetFiles()
            '        Do Until IO.File.Exists(file.FullName) = False
            '            Try
            '                Application.Logger.AddToLog("DELETING - " + file.FullName)
            '                IO.File.Delete(file.FullName)
            '            Catch ex As Exception
            '                Application.Logger.AddToLog(ex.Message)
            '                For i = 0 To 10
            '                    Threading.Thread.Sleep(0)
            '                Next
            '            End Try
            '        Loop
            '    Next
            'End If
        End Sub
        Sub AddCompilerOptions(ByVal options As String)
            If _compilerArguemnts Is Nothing Then
                _compilerArguemnts = options.Trim + " "
            Else
                _compilerArguemnts += options.Trim + " "
            End If
        End Sub
    End Class
End Namespace
