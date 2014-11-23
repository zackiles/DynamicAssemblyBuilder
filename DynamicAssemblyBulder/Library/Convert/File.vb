Imports System.Text
Imports DynamicFramework.Application
Namespace Convert
    Public Class File
        Public Shared Function FileToBase64String(ByVal fileName As String) As String
            Dim stream As IO.Stream = IO.File.Open(fileName, IO.FileMode.Open)
            Dim buffer As Byte() = New Byte(stream.Length) {}
            For i As Integer = 0 To stream.Length - 1
                stream.Read(buffer, 0, stream.Length)
            Next
            stream.Close()
            stream.Dispose()
            Return Global.System.Convert.ToBase64String(buffer)
        End Function

        Public Shared Function Base64StringTextFileToByteArray(ByVal baseStringFile As String) As Byte()
            Dim buffer As Byte() = Nothing
            buffer = Global.System.Convert.FromBase64String(IO.File.ReadAllText(baseStringFile))
            Return buffer
        End Function


        Public Shared Function FilePartsIntoStrings(ByVal fileName As String, ByVal partCount As Integer) As String()
            InternalException.ThrowException(InternalMessage.NotImplementedException, Nothing)
            Return Nothing
        End Function

        Public Shared Sub BytesToFile(ByVal fileBytes As Byte(), ByVal fileName As String, Optional ByVal indexCount As Integer = 0)
            If Not indexCount = 0 Then
                Application.File.Write(fileBytes, fileName)
            Else
                Dim buffer As Byte() = New Byte(indexCount - 1) {}
                For i As Integer = 0 To indexCount - 1
                    buffer(i) = fileBytes(1)
                Next
                Application.File.Write(buffer, fileName)
            End If
        End Sub
        Public Sub StringTFile(ByVal source As String, ByVal fileName As String, Optional ByVal indexCount As Integer = 0)
            If Not indexCount = 0 Then
                Application.File.Write(source, fileName)
            End If
        End Sub

        ''' <summary>
        ''' searching the index of a byte array in a big byte array
        ''' </summary>
        ''' <param name="array">source byte array</param>
        ''' <param name="search">search byte array</param>
        ''' <param name="startIndex">optional start array</param>
        ''' <returns>index of first location or -1</returns>
        ''' <remarks></remarks>
        Public Shared Function IndexOf(ByRef array As Byte(), ByVal search As String, Optional ByVal startIndex As Integer = 0) As Integer
            If (array Is Nothing) OrElse (search Is Nothing) OrElse (search.Length = 0) _
            OrElse (startIndex < 0) OrElse (startIndex > array.Length - search.Length) _
            Then Return -1

            Dim buffer As Byte()
            buffer = Text.Encoding.UTF8.GetBytes(search)

            Return IndexOf(array, Text.Encoding.UTF8.GetString(buffer), startIndex)
        End Function
        ''' <summary>
        ''' searching the index of a byte array in a big byte array
        ''' </summary>
        ''' <param name="array">source byte array</param>
        ''' <param name="search">search byte array</param>
        ''' <param name="startIndex">optional start array</param>
        ''' <returns>index of first location or -1</returns>
        ''' <remarks></remarks>
        Public Shared Function IndexOf(ByRef array As Byte(), ByVal search As Byte(), Optional ByVal startIndex As Integer = 0) As Integer
            If (array Is Nothing) OrElse (search Is Nothing) OrElse (search.Length = 0) _
            OrElse (startIndex < 0) OrElse (startIndex > array.Length - search.Length) _
            Then Return -1
            Dim i, k As Integer
            Dim j As Integer = search.Length - 1
            For i = startIndex To array.Length - search.Length

                For k = 0 To j
                    If array(i + k) <> search(k) Then Exit For
                    If k = j Then Return i
                Next

            Next
            Return -1
        End Function
        ''' <summary>
        ''' Split a byte array in delimiter separated little byte arrays
        ''' </summary>
        ''' <param name="ByteArray">source</param>
        ''' <param name="delimiter">delimiter</param>
        ''' <returns>Array List of byte()</returns>
        ''' <remarks></remarks>
        Public Shared Function Split(ByRef ByteArray As Byte(), ByVal delimiter As String) As ArrayList
            If (ByteArray Is Nothing) OrElse (delimiter Is Nothing) OrElse (delimiter.Length = 0) _
            Then Return New ArrayList

            Dim buffer As Byte()
            buffer = Text.Encoding.UTF8.GetBytes(delimiter)

            Return Split(ByteArray, Text.Encoding.UTF8.GetString(buffer))

        End Function
        ''' </summary>
        ''' <param name="ByteArray">source</param>
        ''' <param name="delimiter">delimiter</param>
        ''' <returns>Array List of byte()</returns>
        ''' <remarks></remarks>
        Public Shared Function Split(ByRef ByteArray As Byte(), ByVal delimiter As Byte()) As ArrayList
            If (ByteArray Is Nothing) OrElse (delimiter Is Nothing) OrElse (delimiter.Length = 0) _
            Then Return New ArrayList

            Dim SplitArray As New ArrayList
            Dim IndexFirst As Integer = 0
            Dim IndexNext As Integer
            Dim len As Integer

            Do
                IndexNext = IndexOf(ByteArray, delimiter, IndexFirst)
                If IndexNext = -1 Then IndexNext = ByteArray.Length

                len = IndexNext - IndexFirst
                Dim buffer(len) As Byte

                Array.Copy(ByteArray, IndexFirst, buffer, 0, len)
                SplitArray.Add(buffer)

                IndexFirst = IndexNext + delimiter.Length


            Loop While IndexFirst < ByteArray.Length - 1

            Return SplitArray

        End Function

        ''' <summary>
        ''' UTF8 Encoding a byte array to a string
        ''' </summary>
        ''' <param name="ByteArray"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function parse2string(ByRef ByteArray As Byte()) As String
            If (ByteArray Is Nothing) OrElse (ByteArray.Length = 0) Then Return ""

            Return New String(Encoding.UTF8.GetChars(ByteArray))
        End Function


    End Class
End Namespace