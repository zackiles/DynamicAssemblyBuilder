Namespace Compiler
    Public Class CodeDomResultProvider
        Public Shared Sub DisplayCompilerResults(ByVal cr As System.CodeDom.Compiler.CompilerResults)
            ' If errors occurred during compilation, output the compiler output and errors. 
            If cr.Errors.Count > 0 Then
                Dim [error] As String = "The following compile error occured :" & vbCr & vbLf
                For Each err As CodeDom.Compiler.CompilerError In cr.Errors
                    [error] += "File: " + err.FileName.ToString + "; Line (" + err.Line.ToString + ") - " + err.ErrorText.ToString + vbLf
                Next
                Application.Logger.AddToLog([error])
            Else
                ' Display information about the compiler's exit code and the generated assembly.
                Application.Logger.AddToLog("Compilation ended for : " + IO.Path.GetFileNameWithoutExtension(cr.PathToAssembly))
                Application.Logger.AddToLog(("Compiler returned with result code: " + cr.NativeCompilerReturnValue.ToString()))
                If cr.PathToAssembly Is Nothing Then
                    Application.Logger.AddToLog("The assembly has been generated in memory.")
                Else
                    Application.Logger.AddToLog(("Path to assembly: " + cr.PathToAssembly))
                End If

            End If
        End Sub
    End Class
End Namespace