Imports System.Windows.Forms
Imports DynamicFramework.Compiler
Imports DynamicFramework.Serializer
Imports DynamicFramework

Public Class AppSettings
   Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        My.Settings.AppSettings_CheckBox1 = CheckBox1.CheckState
        Dim _assemblyBackingStore As New AssemblyBackingStore
        With _assemblyBackingStore
            Dim listOfReferences As New List(Of String)
            For Each itm In ListBox1.Items
                listOfReferences.Add(itm.ToString)
            Next
            _assemblyBackingStore.ReferencedAssemblies = listOfReferences
            Select Case ComboBox1.Text
                Case "NET10"
                    .RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET10
                Case "NET20"
                    .RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET20
                Case "NET35"
                    .RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET35
                Case "NET40"
                    .RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET40
                Case "NET45"
                    .RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET45
                Case Nothing
                Case Else
                    Throw New Exception()
            End Select
            Select Case ComboBox2.Text
                Case "512"
                    .FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align512
                Case "1024"
                    .FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align1024
                Case "2048"
                    .FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align2048
                Case "4096"
                    .FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align4096
                Case "9182"
                    .FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align9182
                Case Nothing
                Case Else
                    Throw New Exception()
            End Select

            Select Case ComboBox3.Text
                Case "STANDARD"
                    .ImageBase = AssemblyDefinition.ImageBaseFlag.STANDARD
                Case "TINY"
                    .ImageBase = AssemblyDefinition.ImageBaseFlag.TINY
                Case "OTHER"
                    .ImageBase = AssemblyDefinition.ImageBaseFlag.OTHER
                Case "MEDIUM"
                    .ImageBase = AssemblyDefinition.ImageBaseFlag.MEDIUM
                Case "EXTENDED"
                    .ImageBase = AssemblyDefinition.ImageBaseFlag.EXTENDED
                Case Nothing
                Case Else
                    Throw New Exception()
            End Select

            Select Case ComboBox4.Text
                Case "LARGE"
                    .StackReserve = AssemblyDefinition.SizeOfStackReserveFlag.LARGE
                Case "STANDARD32BIT"
                    .StackReserve = AssemblyDefinition.SizeOfStackReserveFlag.STANDARD32BIT
                Case "STANDARD64BIT"
                    .StackReserve = AssemblyDefinition.SizeOfStackReserveFlag.STANDARD64BIT
                Case Nothing
                Case Else
                    Throw New Exception()
            End Select

            Select Case ComboBox5.Text
                Case "TwentyPercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.TwentyPercent
                Case "TwentyFivePercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.TwentyFivePercent
                Case "ThirtyPercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.ThirtyPercent
                Case "TenPercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.TenPercent
                Case "FifteenPercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.FifteenPercent
                Case "FiftyPercent"
                    .SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.FiftyPercent
                Case Nothing
                Case Else
                    Throw New Exception()
            End Select
            .RootNamespace = TextBox1.Text
            .FoldMethodBodies = CheckBox2.Checked
            .IncludeDebugInfo = CheckBox3.Checked
            .Optimized = CheckBox4.Checked
            .StripRelocationTable = CheckBox5.Checked
            .ShowConsoleWindow = CheckBox6.Checked
            .StrongSigned = CheckBox7.Checked
            .NoLogo = CheckBox8.Checked
            .SplitResources = CheckBox9.Checked
            .CompressAllResources = CheckBox10.Checked
            .EncryptAllResources = CheckBox11.Checked
            .RoundTripWithIlasm = CheckBox12.Checked
            .StubName = TextBox1.Text
        End With
        Dim _xmlSerializer As New Xml
        _xmlSerializer.Serialize(_assemblyBackingStore, EasyCompiler.Settings.SettingsFileLocation)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CheckBox1.CheckState = My.Settings.AppSettings_CheckBox1
        If IO.File.Exists(EasyCompiler.Settings.SettingsFileLocation) Then
            TextBox2.Text = EasyCompiler.Settings.SettingsFileLocation
            Dim _xmlDeSerializer As New Xml
            Dim _settings As Compiler.AssemblyBackingStore = DirectCast(_xmlDeSerializer.DeSerialize(GetType(AssemblyBackingStore), EasyCompiler.Settings.SettingsFileLocation), Compiler.AssemblyBackingStore)
            With _settings
                ListBox1.Items.Clear()
                For Each refAsm In _settings.ReferencedAssemblies
                    ListBox1.Items.Add(refAsm)
                Next

                Select Case .RuntimeVersion
                    Case AssemblyDefinition.RuntimeVersionInfo.NET10
                        ComboBox1.SelectedIndex = ComboBox1.FindString("NET10")
                    Case AssemblyDefinition.RuntimeVersionInfo.NET20
                        ComboBox1.SelectedIndex = ComboBox1.FindString("NET20")
                    Case AssemblyDefinition.RuntimeVersionInfo.NET35
                        ComboBox1.SelectedIndex = ComboBox1.FindString("NET35")
                    Case AssemblyDefinition.RuntimeVersionInfo.NET40
                        ComboBox1.SelectedIndex = ComboBox1.FindString("NET40")
                    Case AssemblyDefinition.RuntimeVersionInfo.NET45
                        ComboBox1.SelectedIndex = ComboBox1.FindString("NET45")
                    Case Nothing
                    Case Else
                        Throw New Exception()
                End Select

                Select Case .FileAlignment
                    Case AssemblyDefinition.FileAlignmentFlags.Align512
                        ComboBox2.SelectedIndex = ComboBox2.FindString("512")
                    Case AssemblyDefinition.FileAlignmentFlags.Align1024
                        ComboBox2.SelectedIndex = ComboBox2.FindString("1024")
                    Case AssemblyDefinition.FileAlignmentFlags.Align2048
                        ComboBox2.SelectedIndex = ComboBox2.FindString("2048")
                    Case AssemblyDefinition.FileAlignmentFlags.Align4096
                        ComboBox2.SelectedIndex = ComboBox2.FindString("4096")
                    Case AssemblyDefinition.FileAlignmentFlags.Align9182
                        ComboBox2.SelectedIndex = ComboBox2.FindString("9182")
                    Case Nothing
                    Case Else
                        Throw New Exception()
                End Select

                Select Case .ImageBase
                    Case AssemblyDefinition.ImageBaseFlag.STANDARD
                        ComboBox3.SelectedIndex = ComboBox3.FindString("STANDARD")
                    Case AssemblyDefinition.ImageBaseFlag.TINY
                        ComboBox3.SelectedIndex = ComboBox3.FindString("TINY")
                    Case AssemblyDefinition.ImageBaseFlag.OTHER
                        ComboBox3.SelectedIndex = ComboBox3.FindString("OTHER")
                    Case AssemblyDefinition.ImageBaseFlag.MEDIUM
                        ComboBox3.SelectedIndex = ComboBox3.FindString("MEDIUM")
                    Case AssemblyDefinition.ImageBaseFlag.EXTENDED
                        ComboBox3.SelectedIndex = ComboBox3.FindString("EXTENDED")
                    Case Nothing
                    Case Else
                        Throw New Exception()
                End Select


                Select Case .StackReserve
                    Case AssemblyDefinition.SizeOfStackReserveFlag.LARGE
                        ComboBox4.SelectedIndex = ComboBox4.FindString("LARGE")
                    Case AssemblyDefinition.SizeOfStackReserveFlag.STANDARD32BIT
                        ComboBox4.SelectedIndex = ComboBox4.FindString("STANDARD32BIT")
                    Case AssemblyDefinition.SizeOfStackReserveFlag.STANDARD64BIT
                        ComboBox4.SelectedIndex = ComboBox4.FindString("STANDARD64BIT")
                    Case Nothing
                    Case Else
                        Throw New Exception()
                End Select

                Select Case .SplitResourcesSize
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.TwentyPercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("TwentyPercent")
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.TwentyFivePercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("TwentyFivePercent")
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.ThirtyPercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("ThirtyPercent")
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.TenPercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("TenPercent")
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.FifteenPercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("FifteenPercent")
                    Case DynamicFramework.Convert.Strings.PartingLevelPercent.FiftyPercent
                        ComboBox5.SelectedIndex = ComboBox5.FindString("FiftyPercent")
                    Case Nothing
                    Case Else
                        Throw New Exception()
                End Select

                TextBox1.Text = .RootNamespace
                CheckBox2.Checked = .FoldMethodBodies
                CheckBox3.Checked = .IncludeDebugInfo
                CheckBox4.Checked = .Optimized
                CheckBox5.Checked = .StripRelocationTable
                CheckBox6.Checked = .ShowConsoleWindow
                CheckBox7.Checked = .StrongSigned
                CheckBox8.Checked = .NoLogo
                CheckBox9.Checked = .SplitResources
                CheckBox10.Checked = .CompressAllResources
                CheckBox11.Checked = .EncryptAllResources
                CheckBox12.Checked = .RoundTripWithIlasm
            End With
        End If
        IIf(TextBox2.Text.Equals(String.Empty), Button2.Enabled = False, Button2.Enabled = True)
    End Sub

    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked.Equals(True) Then
            ComboBox5.Enabled = True
        Else
            ComboBox5.Enabled = False
        End If

    End Sub

    Private Sub AppSettings_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Not IsNothing(TextBox2.Text) Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Process.Start("notepad", Chr(34) + TextBox2.Text + Chr(34))
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = Nothing
        CheckBox2.Checked = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox5.Checked = False
        CheckBox6.Checked = False
        CheckBox7.Checked = False
        CheckBox8.Checked = False
        CheckBox9.Checked = False
        CheckBox10.Checked = False
        CheckBox11.Checked = False
        CheckBox12.Checked = False
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        ComboBox3.SelectedIndex = -1
        ComboBox4.SelectedIndex = -1
        ComboBox5.SelectedIndex = -1
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ListBox1.Items.Add(TextBox3.Text)
    End Sub
End Class
