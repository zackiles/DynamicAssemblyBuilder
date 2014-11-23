Imports System.IO
Namespace Application
    Public Class File

        Public Shared Function Write(ByRef dataObject As Object, ByRef fileName As String)
            Dim byteBuffer As Byte() = Nothing

            Select Case dataObject.GetType.Name
                Case "String"
                    If IsEnviornmentConfigured(fileName) Then
                        Dim stringBuffer As String = DirectCast(dataObject, String)
                        IO.File.WriteAllText(fileName, stringBuffer)
                        Return True
                    End If
                Case "Byte[]"
                    If IsEnviornmentConfigured(fileName) Then
                        byteBuffer = DirectCast(dataObject, Byte())
                        IO.File.WriteAllBytes(fileName, byteBuffer)
                        Return True
                    End If
                Case "MemoryStream", "Stream"
                    If IsEnviornmentConfigured(fileName) Then
                        Dim fileInfo As New FileInfo(fileName)
                        Using fileStream As FileStream = fileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
                            dataObject.WriteTo(fileStream)

                        End Using
                        Return True
                    End If
            End Select
            Return False
        End Function
        Private Shared Function IsEnviornmentConfigured(ByVal fileName As String) As Boolean
            Try
                If Not Directory.Exists(Path.GetDirectoryName(fileName)) Then
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName))
                End If
                If IO.File.Exists(fileName) Then
                    IO.File.Delete(fileName)
                    'If IsFileLocked(fileName) Then
                    '    InternalException.ThrowException( _
                    '        String.Format("The following needed file is currently in use {0}. Please close any applications accessing it before continuing.", fileName), Nothing)
                    '    Return False
                Else
                End If
                'End If
            Catch ex As Exception
                InternalException.ThrowException(ex)
                Return False
            End Try
            Return True
        End Function
        Private Shared Function IsFileLocked(ByVal fileName As String) As Boolean
            Dim fileInfo As New FileInfo(fileName)
            Dim stream As FileStream = Nothing
            Try
                stream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
            Catch generatedExceptionName As IOException
                Return True
            Finally
                If stream IsNot Nothing Then
                    stream.Close()
                    stream.Dispose()
                End If
            End Try
            Return False
        End Function
    End Class
End Namespace