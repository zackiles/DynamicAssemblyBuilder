Namespace Application
    Public Class Logger
        Private Shared _listOfEntries As List(Of Entry)
        Private Shared _currentEntry As Entry
        Private Shared _firstInstance As Boolean = True
        Private Shared _locking As Object
        Public Shared Sub AddToLog(ByVal msg As String)
            _locking = New Object() {}
            SyncLock _locking
                If _firstInstance = True Then
                    _listOfEntries = New List(Of Entry)
                    _firstInstance = False
                End If
            End SyncLock
            AddToQueue(msg)
        End Sub
        Private Shared Sub AddToQueue(ByVal msg As String)
            _currentEntry = New Entry
            _currentEntry.Time = DateTime.Now
            _currentEntry.LastMessage = msg
            _currentEntry.Reserved1 = Nothing
            SyncLock CType(_listOfEntries, IList).SyncRoot
                _listOfEntries.Add(_currentEntry)
            End SyncLock
        End Sub
        Public Shared Function GetLastEntry() As Entry
            Return _currentEntry
        End Function
        Public Shared Sub PrintAllEntriesToConsole()
            Try
                If Not _listOfEntries Is Nothing Then
                    SyncLock CType(_listOfEntries, IList).SyncRoot
                        Console.Clear()
                        Console.BufferHeight = 200
                        For i As Integer = 0 To _listOfEntries.Count - 1
                            Console.WriteLine("(ENTRY({0}) - {1} - {2}", i, _listOfEntries(i).LastMessage, _listOfEntries(i).Time.ToLongTimeString + vbNewLine + vbNewLine)
                        Next
                    End SyncLock
                Else
                    Console.WriteLine("No entries were found!")
                End If
            Catch
            End Try
        End Sub
        Public Shared Sub PrintAllEntriesToDebug()
            Try
                If Not _listOfEntries Is Nothing Then
                    SyncLock CType(_listOfEntries, IList).SyncRoot
                        For i As Integer = 0 To _listOfEntries.Count - 1
                            Debug.WriteLine("(ENTRY({0}) - {1} - {2}", i, _listOfEntries(i).LastMessage, _listOfEntries(i).Time.ToLongTimeString + vbNewLine + vbNewLine)
                        Next
                    End SyncLock
                Else
                    Debug.WriteLine("No entries were found!")
                End If
            Catch
            End Try
        End Sub
        Public Class Entry
            Public Time As DateTime
            Public LastMessage As String
            Public Reserved1 As Object()
        End Class
    End Class
End Namespace