Imports DynamicFramework.Application
Namespace Decompiler
    Public Class IlDasmAgent
        Private _ilDasmProvider As Application.ShellProcessAgent
        Private _ilDasmFirstOptions As String
        Private Sub AddCompilerOptions(ByVal options As String)
            If _ilDasmFirstOptions Is Nothing Then
                _ilDasmFirstOptions = options.Trim + " "
            Else
                _ilDasmFirstOptions += options.Trim + " "
            End If
        End Sub
        Public Sub New()
            _ilDasmProvider = New Application.ShellProcessAgent
        End Sub
        Public Sub New(ByRef configurationWizard As ConfigurationWizard)

        End Sub
        Private Sub ConfigureDeCompileOnStart()
            _ilDasmProvider.ProcessLocation = Settings.GetIldasmLocation()
            If OutputRtf = True Then
                If OutputToConsoleOnly = False Then
                    AddCompilerOptions("/rtf")
                End If
            End If
            If OutputHtml Then
                If OutputToConsoleOnly Then
                    AddCompilerOptions("/html")
                End If
            End If
            If ShowMethodHexValues Then
                AddCompilerOptions("/bytes")
            End If
            If ShowCustomAttributesInVerbalForm Then
                AddCompilerOptions("/caverbal")
            End If
            If ShowMetaDataTokens Then
                AddCompilerOptions("/tokens")
            End If
            If ShowNamesInSingleQuotes Then
                AddCompilerOptions("/quoteallnames")
            End If
            If ShowRawExceptionCaluses Then
                AddCompilerOptions("/raweh")
            End If
            If ShowSourceLinesAsComments Then
                AddCompilerOptions("/source")
            End If
            If IgnoreCustomAttributes Then
                AddCompilerOptions("/noca")
            End If
            If IgnorePrivateMembers Then
                AddCompilerOptions("/pubonly")
            End If

        End Sub
        Private Sub FullDecompile()
            ConfigureDeCompileOnStart()
            AddCompilerOptions("/all")
            BeginCompilation()
        End Sub
        Private Sub GenerallDecompile()
            InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {Me})
            'ConfigureDeCompileOnStart()
            'BeginCompilation()
        End Sub
        Private Sub LightlDecompile()
            InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {Me})
            'ConfigureDeCompileOnStart()
            'BeginCompilation()
        End Sub
        Private Sub CustomDecompile()
            InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {Me})
            'ConfigureDeCompileOnStart()
            'BeginCompilation()
        End Sub

        Private Sub BeginCompilation()
            If OutputToConsoleOnly Then
                _ilDasmProvider.StartShell(Chr(34) + AssemblyLocation + Chr(34) + " " + _ilDasmFirstOptions + "/text")
            Else
                _ilDasmProvider.StartShell(Chr(34) + AssemblyLocation + Chr(34) + " " + _ilDasmFirstOptions + " /output=" + Chr(34) + OutputFileLocation + Chr(34))
            End If
        End Sub

        Private _OutputFileLocation As String
        Public Property OutputFileLocation As String
            Get
                Return _OutputFileLocation
            End Get
            Set(ByVal value As String)
                If IO.File.Exists(value) Then
                    Logger.AddToLog(String.Format("The file - {0} already exists, overwriting on decompile!", value))
                End If
                _OutputFileLocation = value
            End Set
        End Property

        Private _outputRtf As Boolean = False
        Public Property OutputRtf As Boolean
            Get
                Return _outputRtf
            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    If OutputToConsoleOnly = True Then
                        Logger.AddToLog(String.Format("OutpuRtf and OutPutToConsole cannot both be {0}!", value))
                        Logger.AddToLog(String.Format("RTF will be saved to disk, and console output ignored.!", value))
                        OutputToConsoleOnly = False
                    End If
                End If
                _outputRtf = value
            End Set
        End Property
        Private _outputHtml As Boolean = False
        Public Property OutputHtml As Boolean
            Get
                Return _outputHtml
            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    If OutputToConsoleOnly = True Then
                        Logger.AddToLog(String.Format("OutpuHTML and OutPutToConsoleOnly cannot both be {0}!", value))
                        Logger.AddToLog(String.Format("HTML will be saved to disk, and console output ignored.!", value))
                        OutputToConsoleOnly = False
                    End If
                End If
                _outputHtml = value
            End Set
        End Property
        Private _outputToConsoleOnly As Boolean = False
        Public Property OutputToConsoleOnly As Boolean
            Get
                Return _outputToConsoleOnly
            End Get
            Set(ByVal value As Boolean)
                _outputToConsoleOnly = value
            End Set
        End Property
        Private _showMethodHexValues As Boolean = False
        '   Shows actual bytes, in hexadecimal format, as instruction comments.
        Public Property ShowMethodHexValues As Boolean
            Get
                Return _showMethodHexValues
            End Get
            Set(ByVal value As Boolean)
                _showMethodHexValues = value
            End Set
        End Property
        Private _showCustomAttributesInVerbalForms As Boolean = False
        '   Produces custom attribute blobs in verbal form. The default is binary form.
        Public Property ShowCustomAttributesInVerbalForm As Boolean
            Get
                Return _showCustomAttributesInVerbalForms
            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    If _ignoreCustomAttributes = True Then
                        Application.Logger.AddToLog("CAUTION! - IgnoreCustomAttributes is " + _
                       "turned on, but you have also requested to show custom attributes in verbal form.")
                    End If
                End If
                _showCustomAttributesInVerbalForms = value
            End Set
        End Property
        Private _ignoreCustomAttributes As Boolean = False
        '   Ignores all custom attributes
        Public Property IgnoreCustomAttributes As Boolean
            Get
                Return _ignoreCustomAttributes
            End Get
            Set(ByVal value As Boolean)

                If _showCustomAttributesInVerbalForms = True Then
                    If value = True Then
                        Application.Logger.AddToLog("CAUTION! - CustomAttributesInVerbalForm is " + _
                        "turned on, but you have also requested to ignore ALL custom attributes.")
                    End If
                End If

                _ignoreCustomAttributes = value
            End Set
        End Property
        Private _ignorePrivateMembers As Boolean = False
        '   Ignores all private memebers when decompiling.
        Public Property IgnorePrivateMembers As Boolean
            Get
                Return _ignorePrivateMembers
            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    Application.Logger.AddToLog("CAUTION! Only decompiling public members.")
                End If
                _ignorePrivateMembers = value
            End Set
        End Property
        Private _showNamesInSingleQuotes As Boolean = False
        '   Shows all names in single quotes.
        Public Property ShowNamesInSingleQuotes As Boolean
            Get
                Return _showNamesInSingleQuotes
            End Get
            Set(ByVal value As Boolean)
                _showNamesInSingleQuotes = value
            End Set
        End Property

        Private _showRawExceptionCaluses As Boolean = False
        '   Shows the raw excheption handling clauses.
        Public Property ShowRawExceptionCaluses As Boolean
            Get
                Return _showRawExceptionCaluses
            End Get
            Set(ByVal value As Boolean)
                _showRawExceptionCaluses = value
            End Set
        End Property

        Private _showSourceLinesAsComments As Boolean = False
        '   Shows the original source lines as comments
        Public Property ShowSourceLinesAsComments As Boolean
            Get
                Return _showSourceLinesAsComments
            End Get
            Set(ByVal value As Boolean)
                _showSourceLinesAsComments = value
            End Set
        End Property
        Private _showMetaDataTokens As Boolean = False
        '   Shows the raw meta data tokens.
        Public Property ShowMetaDataTokens As Boolean
            Get
                Return _showMetaDataTokens
            End Get
            Set(ByVal value As Boolean)
                _showMetaDataTokens = value
            End Set
        End Property

        Private _assemblyLocation As String
        Public Property AssemblyLocation As String
            Get
                Return _assemblyLocation
            End Get
            Set(ByVal value As String)
                If IO.File.Exists(value) Then
                    _assemblyLocation = value
                Else
                    InternalException.ThrowException("Unable to find file - " + value, New Object() {Me})
                End If
            End Set
        End Property
        Enum DecompileMode
            GENERAL
            LIGHT
            FULL
            CUSTOM
            OTHER
        End Enum
        Public Overloads Sub DecompileAssembly(ByVal decompileMode As DecompileMode)
            ConfigureDeCompileOnStart()
            Select Case decompileMode
                Case IlDasmAgent.DecompileMode.FULL
                    FullDecompile()
                Case IlDasmAgent.DecompileMode.GENERAL
                    GenerallDecompile()
                Case IlDasmAgent.DecompileMode.LIGHT
                    LightlDecompile()
                Case IlDasmAgent.DecompileMode.CUSTOM
                    CustomDecompile()
                Case IlDasmAgent.DecompileMode.OTHER
                    InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {Me})
            End Select
        End Sub
        Public Overloads Sub DecompileAssembly(ByVal fileLocation As String)
            AssemblyLocation = fileLocation
            GenerallDecompile()
        End Sub
        Public Overloads Sub DecompileAssembly()
            CustomDecompile()
        End Sub
    End Class

End Namespace
