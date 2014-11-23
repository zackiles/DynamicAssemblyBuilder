Namespace Application
    Public Class InternalException
        Public Overloads Shared Sub ThrowException(Optional ByVal ex As Exception = Nothing)
            If Not String.IsNullOrEmpty(ex.Message) Then
                Throw New ApplicationException(ex.Message)
            Else
                Throw New ApplicationException("An unknown error has occured.!")
            End If
        End Sub
        Public Overloads Shared Sub ThrowException(ByVal msg As String, ByVal sender As Object())
            Dim _logger As New Logger
            Application.Logger.AddToLog(msg)
            Throw New ApplicationException(msg)
        End Sub
    End Class
End Namespace
