Namespace Convert
    Public Class Stream
        Public Sub StreamToFile(ByVal stream As IO.Stream, ByVal fileName As String)
            Dim buffer As Byte() = New Byte(stream.Length - 1) {}
            For i As Integer = 0 To stream.Length
                stream.Read(buffer, 0, stream.Length)
            Next
            IO.File.WriteAllBytes(fileName, buffer)
        End Sub
        Public Function CompressStream(ByVal stream As IO.Stream) As IO.Stream
            Return New IO.Compression.DeflateStream(stream, IO.Compression.CompressionMode.Compress, False)
        End Function
        Public Function DeCompressStream(ByVal stream As IO.Stream)
            Return New IO.Compression.DeflateStream(stream, IO.Compression.CompressionMode.Decompress, False)
        End Function
        Public Function StreamToBase64String(ByVal stream As IO.Stream) As String
            Dim buffer As Byte() = New Byte(stream.Length) {}
            For i As Integer = 0 To stream.Length - 1
                stream.Read(buffer, 0, stream.Length)
            Next
            stream.Close()
            stream.Dispose()
            Return Global.System.Convert.ToBase64String(buffer)
        End Function
    End Class
End Namespace