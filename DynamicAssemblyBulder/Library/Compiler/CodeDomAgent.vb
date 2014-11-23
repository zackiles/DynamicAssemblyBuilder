Imports System.Reflection
Imports DynamicFramework.Application

Namespace Compiler
    Public Class CodeDomAgent
        Inherits CompilerBase

        Private _compilerContext As CodeDom.Compiler.CompilerParameters
        Private _VbCodeProvider As Microsoft.VisualBasic.VBCodeProvider
        Private _compilerResults As System.CodeDom.Compiler.CompilerResults
        Public Sub New()
            _compilerContext = New CodeDom.Compiler.CompilerParameters
            _tfc = New System.CodeDom.Compiler.TempFileCollection(Application.Settings.GetWorKSpaceFolder + "\", False)
            _compilerResults = New CodeDom.Compiler.CompilerResults(_tfc)
            _compilerResults.TempFiles = _tfc
            _compilerContext.TempFiles = _tfc
            _tfc.KeepFiles = False
        End Sub
        Overrides Sub ConfigurePreCompile(ByRef assemblyDefinition As AssemblyDefinition)
            _compilerContext.OutputAssembly = assemblyDefinition.Location
            _frameworkVersion = New Collections.Generic.Dictionary(Of String, String)
            _frameworkVersion.Add("CompilerVersion", assemblyDefinition.GetClrVersion)
            '   TO DO - Find out how the damn vbccodeprovider accepts opetioninfer...
            '_frameworkVersion.Add("OptionInfer", "True")
            _VbCodeProvider = New Microsoft.VisualBasic.VBCodeProvider(_frameworkVersion)
            If CompilerIsVerbose = True Then
                AddCompilerOptions("/verbose")
            End If
        End Sub
        Overrides Function ConfigureCompileOnStart(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
            _compilerContext.GenerateInMemory = False
            AddCompilerOptions("/define:_MYTYPE=\""Empty\""")
            If assemblyDefinition.IncludeDebugInfo Then
                AddCompilerOptions("/define:Debug=True /define:Trace=true")
            End If
            Select Case assemblyDefinition.GetSourceTarget
                Case (".dll")
                    _compilerContext.GenerateExecutable = False
                    AddCompilerOptions("/target:library")
                Case (".exe")
                    _compilerContext.GenerateExecutable = True
                    If assemblyDefinition.ShowConsoleWindow = True Then
                        AddCompilerOptions("/target:exe")
                    Else
                        AddCompilerOptions("/target:winexe")
                    End If
                    AddCompilerOptions("/filealign:0x00000200")
                    AddCompilerOptions("/platform:X86")
                    If Not assemblyDefinition.Icon Is Nothing Then
                        AddCompilerOptions("/win32icon:" + assemblyDefinition.Icon)
                    End If
                Case (".netmodule")
            End Select
            Return True
        End Function

        Public Overrides Function AddResources(ByRef assemblyDefinition As AssemblyDefinition) As Boolean

            If Not IsNothing(assemblyDefinition.ManagedResourceList) Then
                For Each resource As ManagedResourceEntity In assemblyDefinition.ManagedResourceList
                    If IO.File.Exists(String.Format("{0}\{1}.resources", Settings.GetWorKSpaceFolder, resource.RootNamespace + ".Resources")) Then
                        _compilerContext.EmbeddedResources.Add(String.Format("{0}\{1}.resources", Settings.GetWorKSpaceFolder, resource.RootNamespace + ".Resources"))
                    End If
                Next
            End If
            If Not IsNothing(assemblyDefinition.UnmanagedResourceList) Then
                For Each resource As ResourceEntity In assemblyDefinition.UnmanagedResourceList
                    If IO.File.Exists(resource.Location) Then
                        _compilerContext.EmbeddedResources.Add(resource.Location)
                    Else
                        InternalException.ThrowException(String.Format("Unable to add resource '{0}' to '{1}", resource.Location, assemblyDefinition.Name), New Object() {Me})
                    End If
                Next
            End If
            If Not IsNothing(assemblyDefinition.EmbeddedAssemblies) Then
                Dim embeddedAssemblies As List(Of String) = assemblyDefinition.EmbeddedAssemblies
                AddCompilerOptions("/libpath:" + Settings.GetWorKSpaceFolder)
                For Each res As String In embeddedAssemblies
                    If IO.File.Exists(res) Then
                        _compilerContext.EmbeddedResources.Add(res)
                        Dim assem As Assembly = Assembly.ReflectionOnlyLoad(IO.File.ReadAllBytes(res))
                        Dim asmName As String = String.Format("{0}.dll", assem.GetName.Name)
                        _compilerContext.ReferencedAssemblies.Add(asmName)
                    Else
                        InternalException.ThrowException(InternalMessage.UnknownFatalException, New Object() {Me})
                    End If
                Next
            End If
            Return True
        End Function
        Overrides Sub BeginCompilation(ByRef assemblyDefinition As AssemblyDefinition)
            _compilerContext.CompilerOptions = _compilerArguemnts
            '    /noconfig fores vbc.exe to ignore the vbc.rsp settings, and use our own that are supplied.
            _compilerResults = _VbCodeProvider.CompileAssemblyFromSource(_compilerContext, _sourceArray)
            CodeDomResultProvider.DisplayCompilerResults(_compilerResults)
            _VbCodeProvider.Dispose()
            _VbCodeProvider = Nothing
            _compilerResults = Nothing
        End Sub
        Public Overrides Sub AddStrongNameKeyPair(ByRef assemblyDefinition As AssemblyDefinition)
            AddCompilerOptions(String.Format("/keyfile:""{0}""", assemblyDefinition.StrongSignKeyPairLocation))
        End Sub

        Public Overrides Sub AddAssemblyReferences(ByRef assemblyDefinition As AssemblyDefinition)
            For Each ref As String In assemblyDefinition.ReferencedAssemblies
                _compilerContext.ReferencedAssemblies.Add(ref)
            Next
        End Sub
    End Class
End Namespace
