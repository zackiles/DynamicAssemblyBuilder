Imports System.Windows.Forms

Public Class DlgAddResource
    Private _resourceLocation As String
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Select Case ComboBox6.Text
                Case "Managed"
                    CompilerProxy.InternalManagedResourceCollection.Add(_resourceLocation, ComboBox7.Text)
                Case "Unmanaged"
                    CompilerProxy.InternalUnmanagedResourceList.Add(_resourceLocation)
            End Select
        Catch
        End Try
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DlgAddResource_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OpenFileDialog1.InitialDirectory = Settings.StubFolder
        OpenFileDialog1.Filter = Nothing
        Dim dResult As DialogResult = OpenFileDialog1.ShowDialog
        If dResult = DialogResult.OK Then
            _resourceLocation = OpenFileDialog1.FileName
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
        If Not IsNothing(ComboBox7.Text) Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox6.SelectedIndexChanged
        Select Case ComboBox6.Text
            Case "Managed"
                ComboBox7.Enabled = True
                If Not ComboBox7.Text = "" Then
                    OK_Button.Enabled = True
                Else
                    OK_Button.Enabled = False
                End If
            Case "Unmanaged"
                ComboBox7.Enabled = False
                OK_Button.Enabled = True
        End Select
    End Sub
End Class
