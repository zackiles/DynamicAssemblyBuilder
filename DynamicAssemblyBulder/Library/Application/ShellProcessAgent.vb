Namespace Application
    Public Class ShellProcessAgent
        Private _processLocation As String
        Private _shellProcessProvider As Process
        Private _shellProcessContext As Diagnostics.ProcessStartInfo
        Sub New()
            ConfigureShell()
        End Sub
        Public Property ProcessLocation As String
            Get
                Return _processLocation
            End Get
            Set(ByVal procLocation As String)
                _processLocation = procLocation
            End Set
        End Property
        Private Sub ConfigureShell()
            _shellProcessProvider = New Process
            _shellProcessContext = New Diagnostics.ProcessStartInfo
            _shellProcessContext.WindowStyle = ProcessWindowStyle.Hidden
            _shellProcessContext.WorkingDirectory = Settings.GetWorKSpaceFolder
            _shellProcessContext.StandardOutputEncoding = Text.Encoding.UTF8
            _shellProcessContext.StandardErrorEncoding = Text.Encoding.UTF8
            _shellProcessContext.UseShellExecute = False
            _shellProcessContext.ErrorDialog = True
            _shellProcessContext.CreateNoWindow = True
            _shellProcessContext.RedirectStandardInput = True
            _shellProcessContext.RedirectStandardOutput = True
            _shellProcessContext.RedirectStandardError = True
        End Sub
        Public Sub StartShell(ByVal arguments As String)
            If Not IsNothing(ProcessLocation) Then
                If IO.File.Exists(ProcessLocation) Then
                    _shellProcessContext.FileName = _processLocation
                    _shellProcessContext.Arguments = arguments
                    _shellProcessProvider.StartInfo = _shellProcessContext
                    _shellProcessProvider.Start()
                    Application.Logger.AddToLog(_shellProcessProvider.StandardOutput.ReadToEnd())
                    _shellProcessProvider.WaitForExit()
                Else
                    InternalException.ThrowException("The ShellProcessAgent was unable to access the process at " + _
                                                     ProcessLocation, New Object() {Me})
                End If
            Else
                InternalException.ThrowException("The ShellProcessAgent process location is not set!" + _
                                                    ProcessLocation, New Object() {Me})
            End If
        End Sub
    End Class
End Namespace
