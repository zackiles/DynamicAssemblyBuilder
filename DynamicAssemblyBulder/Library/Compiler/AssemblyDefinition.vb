Imports DynamicFramework.Application

Namespace Compiler
    Public NotInheritable Class AssemblyDefinition
        Inherits EntityBase
#Region "Initializers / Defaults"
        Private _listOfSourceFiles As List(Of SourceCodeEntity)
        Private _clrVersion As String = "v2.0"
        Private _icon As String
        Private _optimized As Boolean = False
        Private _noLogo As Boolean = False
        Private _shouldFoldMethodBodies As Boolean = False
        Private _stripRelocationTable As Boolean = False
        Private _sizeOfStackReserve As String = "0x00100000"
        Private _comImageHeader As String
        Private _fileAlignment As String = "1024"
        Private _strongSignKeyPairLocation As String
        Private _imageBase As String = "0x00400000"
        Private _strongSigningOn As Boolean = False
        Private _includeDebugInfo As Boolean = False
        Private _sourceTargetFlag As String
        Private _showConsoleWindow As Boolean = False
        Private _referencedAssemblyNames As List(Of String)
        Private _unmanagedResourceList As List(Of ResourceEntity)
        Private _embeddedAssemblies As List(Of String)
        Private _managedResourceList As List(Of ManagedResourceEntity)
        Private _nameOveride As String

        Sub New()
            _listOfSourceFiles = New List(Of SourceCodeEntity)
        End Sub
#End Region
        Public Overrides Property Name As String
            Get
                If IsNothing(_nameOveride) Then
                    If Not IsNothing(_listOfSourceFiles(0).Location) Then
                        Try
                            If IO.File.Exists(_listOfSourceFiles(0).Location) Then
                                _nameOveride = IO.Path.GetFileNameWithoutExtension(_listOfSourceFiles(0).Location)
                            End If
                        Catch
                        End Try
                    End If
                End If
                Return _nameOveride
            End Get
            Set(ByVal value As String)
                _nameOveride = value
            End Set
        End Property
        Public Property UnmanagedResourceList As List(Of ResourceEntity)
            Get
                Return _unmanagedResourceList
            End Get
            Set(ByVal resourceEntityList As List(Of ResourceEntity))
                _unmanagedResourceList = resourceEntityList
            End Set
        End Property
        Public Property ManagedResourceList As List(Of ManagedResourceEntity)
            Get
                Return _managedResourceList
            End Get
            Set(ByVal resourceEntityList As List(Of ManagedResourceEntity))

                For Each resEntity As ManagedResourceEntity In resourceEntityList
                    Dim sourceEntity As New SourceCodeEntity
                    sourceEntity.Location = resEntity.StronglyTypedClassLocation
                    sourceEntity.SetEntityAsObject = IO.File.ReadAllText(resEntity.StronglyTypedClassLocation)
                    AddSourceCode(sourceEntity)
                Next

                _managedResourceList = resourceEntityList
            End Set
        End Property

        Public Property ReferencedAssemblies As List(Of String)
            Get
                If IsNothing(_referencedAssemblyNames) Then
                    _referencedAssemblyNames = New List(Of String)
                    _referencedAssemblyNames.Add("System.dll")
                End If
                Return _referencedAssemblyNames
            End Get
            Set(ByVal referencedAssemblyNames As List(Of String))
                _referencedAssemblyNames = New List(Of String)
                For Each refName As String In referencedAssemblyNames
                    _referencedAssemblyNames.Add(refName)
                Next
            End Set
        End Property
        Public Property EmbeddedAssemblies As List(Of String)
            Get
                Return _embeddedAssemblies
            End Get
            Set(ByVal embeddedAssemblyLocations As List(Of String))
                _embeddedAssemblies = New List(Of String)
                For Each refName As String In embeddedAssemblyLocations
                    _embeddedAssemblies.Add(refName)
                Next
            End Set
        End Property
      
       

        Public Property ShowConsoleWindow As Boolean
            Get
                Return _showConsoleWindow
            End Get
            Set(ByVal value As Boolean)
                _showConsoleWindow = value
            End Set
        End Property

        Enum SourceTarget
            EXE
            DLL
            NETMODULE
            OTHER
        End Enum
        Public Sub SetSourceTarget(ByVal targetFlags As SourceTarget)
            Select Case targetFlags
                Case SourceTarget.EXE
                    _sourceTargetFlag = ".exe"
                Case SourceTarget.DLL
                    _sourceTargetFlag = ".dll"
                Case SourceTarget.NETMODULE
                    _sourceTargetFlag = ".netmodule"
                Case SourceTarget.OTHER
                    InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {Me})
            End Select
        End Sub
        Public Function GetSourceTarget() As String
            If IsNothing(_sourceTargetFlag) Then
                SetSourceTarget(Compiler.AssemblyDefinition.SourceTarget.EXE)
            End If
            Return _sourceTargetFlag
        End Function

        Enum SizeOfStackReserveFlag As UInteger
            STANDARD32BIT = &H100000
            STANDARD64BIT = &H400000
            LARGE = &H800000
        End Enum
        Public Sub SetSizeOfStackReserve(ByVal sizeFlag As SizeOfStackReserveFlag)
            Select Case sizeFlag
                Case SizeOfStackReserveFlag.LARGE
                    _sizeOfStackReserve = "0x00800000"
                Case SizeOfStackReserveFlag.STANDARD64BIT
                    _sizeOfStackReserve = "0x00400000"
                Case SizeOfStackReserveFlag.STANDARD32BIT
                    _sizeOfStackReserve = "0x00100000"
            End Select
        End Sub
        Enum ImageBaseFlag As UInteger
            TINY = &H200000
            STANDARD = &H400000
            MEDIUM = &H1200000
            EXTENDED = &H1400000
            OTHER
        End Enum
        Public Sub SetImageBase(ByVal imageBaseFlag As ImageBaseFlag)
            Select Case imageBaseFlag
                Case imageBaseFlag.TINY
                    _imageBase = "0x00200000"
                Case imageBaseFlag.STANDARD
                    _imageBase = "0x00400000"
                Case imageBaseFlag.MEDIUM
                    _imageBase = "0x01200000"
                Case imageBaseFlag.EXTENDED
                    _imageBase = "0x01400000"
                Case imageBaseFlag.OTHER
                    Application.InternalException.ThrowException(InternalMessage.NotImplementedException, New Object() {GetType(AssemblyDefinition)})
            End Select
        End Sub
        Enum FileAlignmentFlags As Integer
            Align512 = 512
            Align1024 = 1024
            Align2048 = 2048
            Align4096 = 4096
            Align9182 = 8192
        End Enum
        Public Sub SetFileAlignment(ByVal alignment As FileAlignmentFlags)
            Select Case alignment
                Case FileAlignmentFlags.Align512
                    _fileAlignment = "512"
                Case FileAlignmentFlags.Align1024
                    _fileAlignment = "1024"
                Case FileAlignmentFlags.Align2048
                    _fileAlignment = "2048"
                Case FileAlignmentFlags.Align4096
                    _fileAlignment = "4096"
                Case FileAlignmentFlags.Align9182
                    _fileAlignment = "9182"

            End Select
        End Sub
        Enum RuntimeVersionInfo As Integer
            NET10
            NET20
            NET35
            NET40
            NET45
        End Enum
        Public Function SetClrVersion(ByVal versionFlags As RuntimeVersionInfo) As Boolean
            Try
                Select Case versionFlags
                    Case RuntimeVersionInfo.NET10
                        _clrVersion = "v1.0"
                    Case RuntimeVersionInfo.NET20
                        _clrVersion = "v2.0"
                    Case RuntimeVersionInfo.NET35
                        _clrVersion = "v3.5"
                    Case RuntimeVersionInfo.NET40
                        _clrVersion = "v4.0"
                    Case RuntimeVersionInfo.NET45
                        _clrVersion = "v4.5"
                End Select
            Catch ex As Exception
                '   TODO : ADD ERROR HANDLING IF .NET VERSION ISN't SUPPORTED BY LOCAL OS
                Return False
            End Try
            Return True
        End Function
        <Flags()> _
        Public Enum ComImageFlags As UInteger
            Unmanaged = &H0
            ILOnly = &H1
            F32BitsRequired = &H2
            StrongNameSigned = &H8
            Unmanaged2 = &H16
            TrackDebugData = &H10000
        End Enum
        Public Sub SetComeImageHeaderFlags(ByVal corFlags As ComImageFlags)
            Select Case corFlags
                Case ComImageFlags.Unmanaged
                    _comImageHeader += "0x0000000"
                Case ComImageFlags.TrackDebugData
                    _comImageHeader += "0x00010000"
                Case ComImageFlags.ILOnly
                    _comImageHeader += "0x0000001"
                Case ComImageFlags.StrongNameSigned
                    _comImageHeader += "0x0000008"
                Case ComImageFlags.F32BitsRequired
                    _comImageHeader += "0x0000002"
                Case ComImageFlags.Unmanaged2
                    _comImageHeader += "0x0000016"
            End Select
        End Sub

        Public Function GetFileAlignment() As String
            Return _fileAlignment
        End Function
        Public Function GetComImageHeaderFlags() As String
            Return _comImageHeader
        End Function
        Public Function GetClrVersion() As String
            Return _clrVersion
        End Function
        Public Function GetSizeOfStackReserve() As String
            Return _sizeOfStackReserve
        End Function
        Public Function GetImageBase()
            Return _imageBase
        End Function
        Public Property StrongSigningOn As Boolean
            Get
                Return _strongSigningOn
            End Get
            Set(ByVal value As Boolean)
                _strongSigningOn = value
                If _strongSigningOn = True Then
                    SetComeImageHeaderFlags(AssemblyDefinition.ComImageFlags.StrongNameSigned)
                End If
            End Set
        End Property
        Public Property StrongSignKeyPairLocation As String
            Get
                Return _strongSignKeyPairLocation
            End Get
            Set(ByVal snkLocation As String)
                If snkLocation.Contains(".snk") Then
                    _strongSignKeyPairLocation = snkLocation
                Else
                    Application.InternalException.ThrowException("The custom strong name keypair was invalid or not found!", Nothing)
                End If
            End Set
        End Property

        Public Property IncludeDebugInfo As Boolean
            Get
                Return _includeDebugInfo
            End Get
            Set(ByVal value As Boolean)
                _includeDebugInfo = value
            End Set
        End Property
        Public Property StripRelocationTable As Boolean
            Get
                Return _stripRelocationTable
            End Get
            Set(ByVal value As Boolean)
                _stripRelocationTable = value
            End Set
        End Property
        Public Property ShouldFoldMethodBodies As Boolean
            Get
                Return _shouldFoldMethodBodies
            End Get
            Set(ByVal value As Boolean)
                _shouldFoldMethodBodies = value
            End Set
        End Property
        Public Property Optimized As Boolean
            Get
                Return _optimized
            End Get
            Set(ByVal value As Boolean)
                _optimized = value
            End Set
        End Property
        Public Property NoLogo As Boolean
            Get
                Return _noLogo
            End Get
            Set(ByVal value As Boolean)
                _noLogo = value
            End Set
        End Property
        Public Property Icon As String
            Get
                Return _icon
            End Get
            Set(ByVal value As String)
                Try
                    IO.File.ReadAllBytes(value)
                    _icon = value
                Catch ex As Exception
                    Application.InternalException.ThrowException(ex)
                End Try
            End Set
        End Property

#Region "AssemblyDefinition Source"
        Public Overloads Sub AddSourceCode(ByVal listOfSourceFiles As List(Of SourceCodeEntity))
            _listOfSourceFiles = listOfSourceFiles
        End Sub
        Public Overloads Sub AddSourceCode(ByVal sourceFile As SourceCodeEntity)
            _listOfSourceFiles.Add(sourceFile)
        End Sub
        Public Overloads Sub AddSourceCode(ByVal sourceFileArray As SourceCodeEntity())
            For Each itm As SourceCodeEntity In sourceFileArray
                _listOfSourceFiles.Add(itm)
            Next
        End Sub
        Public Function GetSourceCode() As List(Of SourceCodeEntity)
            Return _listOfSourceFiles
        End Function
        Public Function GetSourceCodeFromIndex(ByVal index As Integer) As SourceCodeEntity
            Try
                Return _listOfSourceFiles(index)
            Catch
                Application.InternalException.ThrowException("The source code index provided was invalid.", New Object() {Me})
            End Try
            Return Nothing
        End Function
#End Region
    End Class
End Namespace