Imports System.Xml.Serialization

Namespace Compiler
    <Serializable()> _
    Public Class AssemblyBackingStore
#Region "Initialize"
        Sub New()
            ReferencedAssemblies = New List(Of String)
            StubName = "Stub"
            RuntimeVersion = AssemblyDefinition.RuntimeVersionInfo.NET20
            FileAlignment = AssemblyDefinition.FileAlignmentFlags.Align512
            ImageBase = AssemblyDefinition.ImageBaseFlag.STANDARD
            StackReserve = AssemblyDefinition.SizeOfStackReserveFlag.STANDARD32BIT
            FoldMethodBodies = True
            IncludeDebugInfo = True
            Optimized = True
            StripRelocationTable = False
            ShowConsoleWindow = False
            NoLogo = True
            StrongSigned = True
            SplitResources = False
            SplitResourcesSize = DynamicFramework.Convert.Strings.PartingLevelPercent.TwentyPercent
            CompressAllResources = True
            EncryptAllResources = True
            RoundTripWithIlasm = False
            RootNamespace = Nothing
            _unManagedResourceList = New List(Of String)
            _managedResourceCollection = New List(Of ResourceEntity)

        End Sub
#End Region
#Region "Property Constructs"

        Private _managedResourceCollection As List(Of ResourceEntity)
        Private _unManagedResourceList As List(Of String)
        Public Property ManagedResourceCollection As List(Of ResourceEntity)
            Get
                Return _managedResourceCollection
            End Get
            Set(ByVal value As List(Of ResourceEntity))
                _managedResourceCollection = value
            End Set
        End Property
        Public Property UnManagedResourceList As List(Of String)
            Get
                Return _unManagedResourceList
            End Get
            Set(ByVal value As List(Of String))
                _unManagedResourceList = value
            End Set
        End Property


        Private _stubName As String
        Public Property StubName As String
            Get
                Return _stubName
            End Get
            Set(ByVal value As String)
                _stubName = value
            End Set
        End Property
        Private _sourceTarget As String
        Public Property SourceTarget As String
            Get
                If _sourceTarget Is Nothing Then
                    _sourceTarget = ".exe"
                End If
                Return _sourceTarget
            End Get
            Set(ByVal value As String)
                _sourceTarget = value
            End Set
        End Property
        Private _referencedAssemblies As List(Of String)
        Public Property ReferencedAssemblies As List(Of String)
            Get
                Return _referencedAssemblies
            End Get
            Set(ByVal value As List(Of String))
                _referencedAssemblies = value
            End Set
        End Property

        Private _runtimeVersion As AssemblyDefinition.RuntimeVersionInfo
        Public Property RuntimeVersion As AssemblyDefinition.RuntimeVersionInfo
            Get
                Return AssemblyDefinition.RuntimeVersionInfo.NET20
            End Get
            Set(ByVal value As AssemblyDefinition.RuntimeVersionInfo)
                _runtimeVersion = value
            End Set
        End Property
        Private _fileAlignment As AssemblyDefinition.FileAlignmentFlags
        Public Property FileAlignment As AssemblyDefinition.FileAlignmentFlags
            Get
                Return _fileAlignment
            End Get
            Set(ByVal value As AssemblyDefinition.FileAlignmentFlags)
                _fileAlignment = value
            End Set
        End Property
        Private _imageBase As AssemblyDefinition.ImageBaseFlag
        Public Property ImageBase As AssemblyDefinition.ImageBaseFlag
            Get
                Return _imageBase
            End Get
            Set(ByVal value As AssemblyDefinition.ImageBaseFlag)
                _imageBase = value
            End Set
        End Property
        Private _stackReserv As AssemblyDefinition.SizeOfStackReserveFlag
        Public Property StackReserve As AssemblyDefinition.SizeOfStackReserveFlag
            Get
                Return _stackReserv
            End Get
            Set(ByVal value As AssemblyDefinition.SizeOfStackReserveFlag)
                _stackReserv = value
            End Set
        End Property
        Private _foldMethodBodies As Boolean
        Public Property FoldMethodBodies As Boolean
            Get
                Return _foldMethodBodies
            End Get
            Set(ByVal value As Boolean)
                _foldMethodBodies = value
            End Set
        End Property
        Private _includeDebugInfo As Boolean
        Public Property IncludeDebugInfo As Boolean
            Get
                Return _includeDebugInfo
            End Get
            Set(ByVal value As Boolean)
                _includeDebugInfo = value
            End Set
        End Property
        Private _optimized As Boolean
        Public Property Optimized As Boolean
            Get
                Return _optimized
            End Get
            Set(ByVal value As Boolean)
                _optimized = value
            End Set
        End Property
        Private _stripRelocationTable As Boolean
        Public Property StripRelocationTable As Boolean
            Get
                Return _stripRelocationTable
            End Get
            Set(ByVal value As Boolean)
                _stripRelocationTable = value
            End Set
        End Property
        Private _showConsoleWindow As Boolean
        Public Property ShowConsoleWindow As Boolean
            Get

                Return _showConsoleWindow
            End Get
            Set(ByVal value As Boolean)
                _showConsoleWindow = value
            End Set
        End Property
        Private _noLogo As Boolean
        Public Property NoLogo As Boolean
            Get
                Return _noLogo
            End Get
            Set(ByVal value As Boolean)
                _noLogo = value
            End Set
        End Property
        Private _strongSigned As Boolean
        Public Property StrongSigned As Boolean
            Get
                Return _strongSigned
            End Get
            Set(ByVal value As Boolean)
                _strongSigned = value
            End Set
        End Property
#End Region
#Region "Method Constructs"
        Private _SplitResources As Boolean
        Public Property SplitResources As Boolean
            Get
                Return _SplitResources
            End Get
            Set(ByVal value As Boolean)
                _SplitResources = value
            End Set
        End Property
        Private _splitResourcesSize As DynamicFramework.Convert.Strings.PartingLevelPercent
        Public Property SplitResourcesSize As DynamicFramework.Convert.Strings.PartingLevelPercent
            Get
                Return _splitResourcesSize
            End Get
            Set(ByVal value As DynamicFramework.Convert.Strings.PartingLevelPercent)
                _splitResourcesSize = value
            End Set
        End Property
        Private _compressAllResources As Boolean
        Public Property CompressAllResources As Boolean
            Get
                Return _compressAllResources
            End Get
            Set(ByVal value As Boolean)
                _compressAllResources = value
            End Set
        End Property
        Private _encryptAllResources As Boolean
        Public Property EncryptAllResources As Boolean
            Get
                Return _encryptAllResources
            End Get
            Set(ByVal value As Boolean)
                _encryptAllResources = value
            End Set
        End Property
        Private _roundTripWithIlasm As Boolean
        Public Property RoundTripWithIlasm As Boolean
            Get
                Return _roundTripWithIlasm
            End Get
            Set(ByVal value As Boolean)
                _roundTripWithIlasm = value
            End Set
        End Property
        Private _rootNamespace As String
        Public Property RootNamespace As String
            Get
                Return _rootNamespace
            End Get
            Set(ByVal value As String)
                _rootNamespace = value
            End Set
        End Property
#End Region
    End Class
End Namespace