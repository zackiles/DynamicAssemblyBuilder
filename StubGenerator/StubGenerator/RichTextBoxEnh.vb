
#Region "Copyright © 2008 Ashu Fouzdar. All rights reserved."
'
'Copyright © 2008 Ashu Fouzdar. All rights reserved.
'
'Redistribution and use in source and binary forms, with or without
'modification, are permitted provided that the following conditions
'are met:
'
'1. Redistributions of source code must retain the above copyright
'   notice, this list of conditions and the following disclaimer.
'2. Redistributions in binary form must reproduce the above copyright
'   notice, this list of conditions and the following disclaimer in the
'   documentation and/or other materials provided with the distribution.
'3. The name of the author may not be used to endorse or promote products
'   derived from this software without specific prior written permission.
'
'THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR
'IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
'OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
'IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
'INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
'NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
'DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
'THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
'(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
'THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
' 
#End Region

Imports System.Windows.Forms

Public Class RichTextBoxEnh
    Private WithEvents findDialog As DlgFind
    Private foundIndex As Integer
    Private foundWord As String

    Private Sub findDialog_Find(ByVal findWhat As String, ByVal findOption As RichTextBoxFinds) Handles findDialog.Find
        Dim findIndex As Integer = 0
        If findWhat.Equals(foundWord) Then findIndex = foundIndex
        If findOption And RichTextBoxFinds.Reverse = RichTextBoxFinds.Reverse Then
            findIndex = Me.Find(findWhat, 0, findIndex, findOption)
        Else
            findIndex = Me.Find(findWhat, findIndex, findOption)
        End If
        If findIndex > 0 Then
            foundWord = findWhat
            If findOption And RichTextBoxFinds.Reverse = RichTextBoxFinds.Reverse Then
                foundIndex = findIndex
            Else
                foundIndex = findIndex + findWhat.Length
            End If
        End If
    End Sub

    Private Sub RichTextBoxEnh_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.Modifiers = Keys.Control And e.KeyCode = Keys.F Then
            If findDialog Is Nothing Then findDialog = New DlgFind()
            Me.HideSelection = False
            findDialog.ShowDialog()
        End If
    End Sub

End Class

