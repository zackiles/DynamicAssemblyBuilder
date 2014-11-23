Public Class SourceCodeTools
    Public Sub VerifyProtableExecutable()
        '   TO DO - ADD CODE FOR PE VERIFY
    End Sub

    Public Shared Sub QuickCompileNoResource(ByVal sources As String())
        Dim c1 As New Application.ConfigurationWizard
        Dim fileInSourceArray As String() = New String(sources.Count - 1) {}
        For i As Integer = 0 To sources.Count - 1
            fileInSourceArray(i) = sources(i)
        Next
        c1.CompilerAgent = Application.ConfigurationWizard.CompilerAgentType.CodeDomAgent
        c1.SourceCodeFileCollection = fileInSourceArray
        c1.Start()
    End Sub


End Class
