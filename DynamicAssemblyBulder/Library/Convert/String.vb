Imports System.Text

Namespace Convert
    Public Class Strings
        Enum PartingLevelPercent
            FiftyPercent = 50.0F
            ThirtyPercent = 30.0F
            TwentyFivePercent = 25.0F
            TwentyPercent = 20.0F
            FifteenPercent = 15.0F
            TenPercent = 10.0F
        End Enum
        Private Shared Function StringArrayToList(ByVal inArray As String()) As List(Of String)
            If Not IsNothing(inArray) Then
                Dim listHolder As New List(Of String)
                For i As Integer = 0 To inArray.Count - 1
                    listHolder.Add(inArray(i))
                Next
                Return listHolder
            End If
            Return Nothing
        End Function
        Private Shared Function ListToStringArray(ByVal inList As List(Of String)) As String()
            If Not IsNothing(inList) Then
                Dim arrayHolder As String() = New String(inList.Count - 1) {}
                For i As Integer = 0 To inList.Count - 1
                    arrayHolder(i) = inList(i)
                Next
                Return arrayHolder
            End If
            Return Nothing
        End Function
        Public Shared Sub Base64StringToFile(ByVal baseString As String, ByVal location As String)
            Dim buffer As Byte() = Global.System.Convert.FromBase64String(baseString)
            IO.File.WriteAllBytes(location, buffer)
        End Sub
        Public Shared Function StringToParts(ByVal stringToSplit As String, ByVal partingLevelPercent As PartingLevelPercent) As String()
            Dim arrList As New ArrayList()
            Dim splitindexCount As Integer = Nothing
            Dim partingPercent As Single = Nothing
            Select Case partingLevelPercent
                Case partingLevelPercent.FiftyPercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.5F)
                Case partingLevelPercent.ThirtyPercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.3F)
                Case partingLevelPercent.TwentyFivePercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.25F)
                Case partingLevelPercent.TwentyPercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.2F)
                Case partingLevelPercent.FifteenPercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.35F)
                Case partingLevelPercent.TenPercent
                    splitindexCount = Global.System.Convert.ToInt32(stringToSplit.Length * 0.1F)
            End Select
            Try
                For i As Integer = 0 To stringToSplit.Length - 1 Step splitindexCount
                    arrList.Add(stringToSplit.Substring(i, splitindexCount))
                Next
            Catch
            End Try
            If Not stringToSplit.Length Mod splitindexCount = 0 Then
                arrList.Add(stringToSplit.Substring(arrList.Count * splitindexCount))
            End If
            Dim arrayOfStrings As String() = New String(arrList.Count) {}
            For i As Integer = 0 To arrList.Count - 1
                arrayOfStrings(i) = arrList.Item(i)
            Next
            Return arrayOfStrings
        End Function
        ''' <summary>
        ''' UTF8 Encoding a byte array to a string
        ''' </summary>
        ''' <param name="ByteArray"></param>
        ''' <param name="Index">Startindex</param>
        ''' <param name="Count">Optional Count</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BytesToString(ByRef ByteArray As Byte(), ByVal Index As Integer, Optional ByVal Count As Integer = -1) As String
            If (ByteArray Is Nothing) OrElse (ByteArray.Length = 0) OrElse _
            (ByteArray.Length < Index) OrElse (Index < 0) Then Return ""
            If (Count < -1) OrElse (Count = 0) Then Return ""

            If Count = -1 Then Count = ByteArray.Length - Index
            If Count + Index > ByteArray.Length Then Count = ByteArray.Length - Index

            Return New String(Encoding.UTF8.GetChars(ByteArray, Index, Count))
        End Function
    End Class
End Namespace
