Imports DynamicFramework.Application
Imports System.Reflection

Namespace Compiler
    Public Class IlasmAgent
        Inherits CompilerBase
        Private _ilAsmProvider As Application.ShellProcessAgent
        Public Sub New()
            Initialize()
            _ilAsmProvider = New Application.ShellProcessAgent
        End Sub
        Overrides Sub BeginCompilation(ByRef assemblyDefinition As AssemblyDefinition)
            _sourceFiles = assemblyDefinition.GetSourceCode
            If _sourceFiles.Count > 1 Then
                Logger.AddToLog("WARNING : Multiple source files passed to IlasmAgent...")
                Logger.AddToLog(String.Format("IldasmAgent will only compile the first source file loated at - {0}{1}", _
                                              vbNewLine, _sourceFiles(0).Location))
            End If
            _ilAsmProvider.StartShell(Chr(34) + _sourceFiles(0).Location + Chr(34) + " " + _compilerArguemnts + " /OUT=" + Chr(34) + assemblyDefinition.Location + Chr(34))
        End Sub
        Public Overrides Sub AddStrongNameKeyPair(ByRef assemblyDefinition As AssemblyDefinition)
            AddCompilerOptions(String.Format("/key:""{0}""", assemblyDefinition.StrongSignKeyPairLocation))
        End Sub
        Overrides Sub ConfigurePreCompile(ByRef assemblyDefinition As AssemblyDefinition)
            IO.File.WriteAllBytes(Settings.GetWorKSpaceFolder + "\fusion.dll", Application.ResourceCache.GetResourceFileAsBytes("fusion"))
            _ilAsmProvider.ProcessLocation = Settings.GetIlasmLocation
        End Sub
        Overrides Function ConfigureCompileOnStart(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
            If assemblyDefinition.ShouldFoldMethodBodies = True Then
                AddCompilerOptions("/fold")
            End If
            If Not assemblyDefinition.GetComImageHeaderFlags Is Nothing Then
                AddCompilerOptions("/flags=" + assemblyDefinition.GetComImageHeaderFlags)
            End If
            If assemblyDefinition.IncludeDebugInfo = True Then
                AddCompilerOptions("/debug=OPT /pdb")
            End If
            If Not assemblyDefinition.GetFileAlignment Is Nothing Then
                AddCompilerOptions("/alignment=" + assemblyDefinition.GetFileAlignment)
            End If
            If Not assemblyDefinition.GetImageBase Is Nothing Then
                AddCompilerOptions("/base=" + assemblyDefinition.GetImageBase)
            End If
            If Not assemblyDefinition.GetSizeOfStackReserve Is Nothing Then
                AddCompilerOptions("/stack=" + assemblyDefinition.GetSizeOfStackReserve)
            End If
            If assemblyDefinition.Optimized = True Then
                AddCompilerOptions("/optimize")
            End If
            If assemblyDefinition.StripRelocationTable = True Then
                AddCompilerOptions("/stripreloc")
            End If
            If assemblyDefinition.NoLogo = True Then
                AddCompilerOptions("/nologo")
            End If
            Select Case assemblyDefinition.GetSourceTarget
                Case (".dll")
                    AddCompilerOptions("/dll")
                Case (".exe")
                    If assemblyDefinition.ShowConsoleWindow = True Then
                        AddCompilerOptions("/subsystem=3 /exe")
                    Else
                        AddCompilerOptions("/subsystem=2 /exe")
                    End If
                    'TO DO ADD ICON CODE
                    'If Not assemblyDefinition.Icon Is Nothing Then
                    '    AddCompilerOptions("/win32icon:" + Chr(34) + assemblyDefinition.Icon + Chr(34))
                    'End If
                Case (".netmodule")
            End Select

        End Function

        Overrides Function AddResources(ByRef assemblyDefinition As AssemblyDefinition) As Boolean
            Dim managedList As List(Of ManagedResourceEntity)
            If Not assemblyDefinition.ManagedResourceList Is Nothing Then
                managedList = assemblyDefinition.ManagedResourceList
                If managedList.Count > 1 Then
                    Logger.AddToLog("Multiple managed resource files in AssemblyDefinition detected...")
                    Logger.AddToLog(String.Format("Ildasm will only compile the first Resource file loated at - {0}{1}", _
                                                  vbNewLine, managedList(0).Location))
                End If
                If IO.File.Exists(managedList(0).Location) Then
                    AddCompilerOptions(String.Format("/resource:""{0}""", managedList(0).Location))
                Else
                    InternalException.ThrowException(String.Format("Unable to add resource '{0}' to '{1}", managedList(0).Location, assemblyDefinition.Location), New Object() {Me})
                End If
            End If


            Dim embeddedAssemblies As List(Of String) = assemblyDefinition.EmbeddedAssemblies
            If Not embeddedAssemblies Is Nothing Then
                For Each resource As String In embeddedAssemblies
                    If IO.File.Exists(resource) Then
                        ' TO DO ADD ILASM EMBEDEDRESOURCE CODE
                        Dim assem As Assembly = Assembly.ReflectionOnlyLoad(IO.File.ReadAllBytes(resource))
                        Dim asmName As String = String.Format("{0}.dll", assem.GetName.Name)
                    Else ' TO DO ADD ILASM REFERENCEDASSEMBLY CODE / SEE AddAssemblyReferences
                        InternalException.ThrowException(InternalMessage.UnknownFatalException, New Object() {Me})
                    End If
                Next
            End If
            Return True
        End Function

        Public Overrides Sub AddAssemblyReferences(ByRef assemblyDefinition As AssemblyDefinition)
            ' TO DO ADD ILASM REFERENCEDASSEMBLY CODE
        End Sub
    End Class
End Namespace
