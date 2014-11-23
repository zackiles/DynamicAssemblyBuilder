Public Class Settings
    Public Shared SettingsFileLocation As String = String.Format("{0}\{1}", IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location), "StubSettings.xml")
    Public Shared OutputFolder As String = "C:\BoxedCompiler\Output"
    Public Shared StubFolder As String = AppDomain.CurrentDomain.BaseDirectory + "Stubs"
End Class
