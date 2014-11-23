Namespace Compiler
    Public Class StrongNameAgent
        Private Shared _keyPairProvider As Application.ShellProcessAgent
        Private Shared _outputKeyPairLocation As String
        Public Shared Function GenerateKeyPair(ByRef assemblyDefinition As AssemblyDefinition) As String
              _keyPairProvider = New Application.ShellProcessAgent
            _keyPairProvider.ProcessLocation = Application.Settings.GetAssemblySignerLocation
            _outputKeyPairLocation = New String(IO.Path.GetFileNameWithoutExtension(assemblyDefinition.Location) + ".snk")
            _keyPairProvider.StartShell(" -k " + Chr(34) + _outputKeyPairLocation + Chr(34))
            _outputKeyPairLocation = Application.Settings.GetWorKSpaceFolder + "\" + _outputKeyPairLocation
            Return _outputKeyPairLocation
        End Function
    End Class

End Namespace