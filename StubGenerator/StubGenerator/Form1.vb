Imports DynamicFramework
Imports DynamicFramework.Compiler

Public Class Form1
    Private _currentSourceFileLocation As String = Nothing
    Private _currentSourceBuffer As String = Nothing
    Private Sub ClearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        RichTextBoxEnh1.Clear()
    End Sub
    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        OpenFileDialog1.InitialDirectory = Settings.StubFolder
        OpenFileDialog1.Filter = "Text Files (*.txt)|*.txt"
        Dim dResult As DialogResult = OpenFileDialog1.ShowDialog
        If dResult = DialogResult.OK Then
            _currentSourceFileLocation = OpenFileDialog1.FileName
            My.Settings.LastStubFileLocation = _currentSourceFileLocation
            My.Settings.Save()
            TextBox_SourceFileLocation.Text = _currentSourceFileLocation
            _currentSourceBuffer = Nothing
            RichTextBoxEnh1.Text = IO.File.ReadAllText(_currentSourceFileLocation)
            SaveLastFiles(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        SaveFileDialog1.InitialDirectory = Settings.StubFolder
        SaveFileDialog1.Filter = "TXT's (*.txt)|*.txt"
        Dim dResult As DialogResult = SaveFileDialog1.ShowDialog
        If dResult = DialogResult.OK Then
            _currentSourceFileLocation = SaveFileDialog1.FileName
            TextBox_SourceFileLocation.Text = SaveFileDialog1.FileName
            IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBoxEnh1.Text)
            SaveLastFiles(SaveFileDialog1.FileName)
            My.Settings.LastStubFileLocation = SaveFileDialog1.FileName
            _currentSourceFileLocation = SaveFileDialog1.FileName
            _currentSourceBuffer = Nothing

        End If
    End Sub
    Private Sub LoadMySettings()
        Dim buffer As String = Nothing
        If My.Settings.AppSettings_CheckBox1 = CheckState.Checked Then
            buffer = My.Settings.LastStubFileLocation
            If Not IsNothing(buffer) Then
                If Not IO.File.Exists(buffer) = False Then
                    _currentSourceFileLocation = buffer
                    TextBox_SourceFileLocation.Text = _currentSourceFileLocation
                    RichTextBoxEnh1.Text = IO.File.ReadAllText(_currentSourceFileLocation)
                Else
                    MsgBox("The stub you have chosen to load on startup could not be found!")
                    My.Settings.AppSettings_CheckBox1 = CheckState.Unchecked
                    My.Settings.LastStubFileLocation = ""
                    My.Settings.Save()
                End If
            End If
        End If
    End Sub
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        LoadMySettings()
    End Sub
    Private Sub LoadLastFiles()
        If Not My.Settings.LastFile.Equals(String.Empty) Then
            NoneToolStripMenuItem.Visible = False
            Dim lastFileCollection As String = My.Settings.LastFile
            Dim fileLocationArray As String() = My.Settings.LastFile.Split("*")
            If Not fileLocationArray(0).Equals(String.Empty) Then
                ToolStripMenuItem2.Text = "1:" + fileLocationArray(0)
                ToolStripMenuItem2.Visible = True
            End If
            If Not fileLocationArray(1).Equals(String.Empty) Then
                ToolStripMenuItem3.Text = "2:" + fileLocationArray(1)
                ToolStripMenuItem3.Visible = True
            End If
            If Not fileLocationArray(2).Equals(String.Empty) Then
                ToolStripMenuItem4.Visible = True
                ToolStripMenuItem4.Text = "3:" + fileLocationArray(2)
            End If
            If Not fileLocationArray(3).Equals(String.Empty) Then
                ToolStripMenuItem5.Visible = True
                ToolStripMenuItem5.Text = "4:" + fileLocationArray(3)
            End If
            If Not fileLocationArray(4).Equals(String.Empty) Then
                ToolStripMenuItem6.Visible = True
                ToolStripMenuItem6.Text = "5:" + fileLocationArray(4)
            End If
        End If
    End Sub
    Private Sub SaveLastFiles(ByVal newFile As String)
        Dim fileLocationArray As String() = Nothing
        If Not My.Settings.LastFile.Equals(String.Empty) Then
            fileLocationArray = My.Settings.LastFile.Split("*")
            If fileLocationArray(0).Equals(newFile) Then
                Return
            End If
        Else
            fileLocationArray = New String(4) {}
        End If
        fileLocationArray(4) = fileLocationArray(3)
        fileLocationArray(3) = fileLocationArray(2)
        fileLocationArray(1) = fileLocationArray(0)
        fileLocationArray(0) = newFile
        Dim buffer As String = Nothing
        For Each s As String In fileLocationArray
            buffer += s + "*"
        Next
        My.Settings.LastFile = buffer
        My.Settings.Save()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If IsNothing(_currentSourceFileLocation) Then
            SaveAsToolStripMenuItem_Click(Me, Nothing)
            Return
        End If
        Try
            IO.File.WriteAllText(_currentSourceFileLocation, RichTextBoxEnh1.Text)
        Catch
            MsgBox("There was an error writing to the file, please ensure it's not open in another application.")
        End Try
    End Sub
    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Dim settingsDialog As New AppSettings()
        settingsDialog.ShowDialog(Me)
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(RichTextBoxEnh1.Text) Then
            MsgBox("No sources have been loaded to generate!")
            Return
        End If
        SaveFileDialog1.InitialDirectory = Settings.OutputFolder
        SaveFileDialog1.Filter = "EXE's (*.exe)|*.exe"
        Dim dResult As DialogResult = SaveFileDialog1.ShowDialog
        Dim tempfile As String
        If dResult = DialogResult.OK Then
            If IO.File.Exists(EasyCompiler.Settings.SettingsFileLocation) Then
                Dim _xmlDeSerializer As New Serializer.Xml
                Dim _assemblyStore As Compiler.AssemblyBackingStore = DirectCast( _
                    _xmlDeSerializer.DeSerialize(GetType(AssemblyBackingStore), _
                    EasyCompiler.Settings.SettingsFileLocation), Compiler.AssemblyBackingStore)
                Dim _compilerProxy As New EasyCompiler.CompilerProxy
                With _compilerProxy
                    .OutputLocation = SaveFileDialog1.FileName
                    .StubSourceCode = RichTextBoxEnh1.Text
                    .AssemblyStore = _assemblyStore
                    tempfile = Settings.OutputFolder + "\" + _assemblyStore.RootNamespace + ".exe"
                    If Not ListBox1.Items.Count < 0 Then
                        Dim fileLocationArray As String() = New String(ListBox1.Items.Count - 1) {}
                        For i As Integer = 0 To ListBox1.Items.Count - 1
                            fileLocationArray(i) = ListBox1.Items.Item(i).ToString
                        Next
                        .ResourceFileCollection = fileLocationArray
                    End If
                End With
                If CheckBox1.Checked = True Then
                    _compilerProxy.StartProxy()
                Else
                    _compilerProxy.StartProxy(False)
                End If

                Try
                    If IO.File.Exists(tempfile) Then
                        IO.File.Copy(tempfile, SaveFileDialog1.FileName)
                        IO.File.Delete(tempfile)
                    End If
                Catch
                End Try
                If Not IO.File.Exists(SaveFileDialog1.FileName) Then
                    MsgBox("There was an error compiling, please check the debug log.")
                End If
            End If
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RichTextBoxEnh1.Text = _currentSourceBuffer
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        _currentSourceBuffer = RichTextBoxEnh1.Text
        Dim source As String = RichTextBoxEnh1.Text
        StubSouceUtil.Configure(source)
        RichTextBoxEnh1.Text = source
    End Sub
    Private Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.DropDownOpened
        LoadLastFiles()
    End Sub
    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        LastFileToolStripValidate(ToolStripMenuItem2)
    End Sub
    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        LastFileToolStripValidate(ToolStripMenuItem3)
    End Sub
    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        LastFileToolStripValidate(ToolStripMenuItem4)
    End Sub
    Private Sub ToolStripMenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem6.Click
        LastFileToolStripValidate(ToolStripMenuItem5)
    End Sub
    Private Sub LastFileToolStripValidate(ByVal toolStrip As ToolStripMenuItem)
        If IO.File.Exists(toolStrip.Text.Remove(0, 2)) Then
            RichTextBoxEnh1.Text = IO.File.ReadAllText(toolStrip.Text.Remove(0, 2))
        Else
            MsgBox("The file could not be found!")
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim result As DialogResult = DlgAddResource.ShowDialog(Me)
        If result = DialogResult.OK Then
            ListBox1.Items.Clear()
            For Each res As KeyValuePair(Of String, String) In CompilerProxy.InternalManagedResourceCollection
                ListBox1.Items.Add(res.Key)
            Next
            For Each res As String In CompilerProxy.InternalUnmanagedResourceList
                ListBox1.Items.Add(res)
            Next
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        TextBox_SourceFileLocation.Text = Nothing
        _currentSourceFileLocation = Nothing
        _currentSourceBuffer = Nothing
        RichTextBoxEnh1.Text = Nothing
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim listBoxSelectedIndex As Integer = ListBox1.SelectedIndex
        If ListBox1.Items.Count = 0 Then
            MsgBox("No resources have been added to remove.")
            Return
        ElseIf listBoxSelectedIndex = -1 Then
            MsgBox("Please select an item first.")
            Return
        End If

        If Not CompilerProxy.InternalManagedResourceCollection.Remove(ListBox1.Items.Item(listBoxSelectedIndex)) Then
            CompilerProxy.InternalUnmanagedResourceList.Remove(ListBox1.Items.Item(listBoxSelectedIndex))
        End If

        ListBox1.Items.RemoveAt(listBoxSelectedIndex)
    End Sub
End Class

