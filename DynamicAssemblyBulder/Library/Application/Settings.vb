Namespace Application
    Public Class Settings
        Private Shared _applicationFolder As String
        Private Shared _workSpaceLocation As String
        Private Shared _locking As Object
        Private Const _tempFolderName = "\Temp"
        Private Const _outputFolderName = "\Output"
        Private Const _configurationFolderName = "\Configuration"
        Private Shared _initilized As Boolean = False
        Private Shared Sub InitializeClass()
            If IsNothing(_workSpaceLocation) Then
                _workSpaceLocation = Application.Settings.GetApplicationFolder + _tempFolderName
                Try
                    Application.Logger.AddToLog(String.Format("Workspace at : {0} and application path at : {1}", _workSpaceLocation, _applicationFolder))
                    If Not IO.Directory.Exists(_workSpaceLocation) Then
                        IO.Directory.CreateDirectory(_workSpaceLocation)
                    End If
                Catch ex As Exception
                    InternalException.ThrowException(ex)
                End Try
                Try
                    If IsNothing(_outPutFolder) Then
                        _outPutFolder = Application.Settings.GetApplicationFolder + _outputFolderName
                        If Not IO.Directory.Exists(_outPutFolder) Then
                            IO.Directory.CreateDirectory(_outPutFolder)
                        End If
                    End If
                Catch ex As Exception
                    InternalException.ThrowException(ex)
                End Try
            End If
        End Sub
        Public Shared Function GetApplicationFolder() As String
            If _applicationFolder Is Nothing Then
                _applicationFolder = "C:\BoxedCompiler"
            End If
            Return _applicationFolder
        End Function
        Public Shared Function GetWorKSpaceFolder() As String
            _locking = New Object() {}
            SyncLock _locking
                CheckIfInitialized()
            End SyncLock
            Return _workSpaceLocation
        End Function
        Private Shared _outPutFolder As String
        Public Shared Function GetOutPutFolder() As String
            _locking = New Object() {}
            SyncLock _locking
                CheckIfInitialized()
            End SyncLock
            Return _outPutFolder
        End Function
        Public Shared Function GetCacheFolder() As String
            Return GetWorKSpaceFolder() + _configurationFolderName
        End Function
        Public Shared Function GetIlasmLocation() As String
            Dim fileLocation As String = String.Format(GetWorKSpaceFolder() + "\{0}.exe", "ilasm")
            If Not IO.File.Exists(fileLocation) Then
                Application.File.Write(ResourceCache.GetResourceFileAsBytes("ilasm"), fileLocation)
            End If
            Return fileLocation
        End Function
        Public Shared Function GetIldasmLocation() As String
            Dim fileLocation As String = String.Format(GetWorKSpaceFolder() + "\{0}.exe", "ildasm")
            If Not IO.File.Exists(fileLocation) Then
                Application.File.Write(ResourceCache.GetResourceFileAsBytes("ildasm"), fileLocation)
            End If
            Return fileLocation
        End Function
        Public Shared Function GetResGenLocation() As String
            Dim fileLocation As String = String.Format(GetWorKSpaceFolder() + "\{0}.exe", "resgen")
            If Not IO.File.Exists(fileLocation) Then
                Application.File.Write(ResourceCache.GetResourceFileAsBytes("resgen"), fileLocation)
            End If
            Return fileLocation
        End Function
        Public Shared Function GetAssemblySignerLocation() As String
            Dim fileLocation As String = String.Format(GetWorKSpaceFolder() + "\{0}.exe", "sn")
            If Not IO.File.Exists(fileLocation) Then
                Application.File.Write(ResourceCache.GetResourceFileAsBytes("sn"), fileLocation)
            End If
            Return fileLocation
        End Function
        Public Shared Function GetSourceCodeLocation(ByVal sourceNameNoExtension As String) As String
            Dim fileLocation As String = String.Format(GetWorKSpaceFolder() + "\{0}.txt", sourceNameNoExtension)
            If Not IO.File.Exists(fileLocation) Then
                Application.File.Write(ResourceCache.GetResourceFileAsString(String.Format("{0}", sourceNameNoExtension)), fileLocation)
            End If
            Return fileLocation
        End Function
        Public Shared ReadOnly Property GetXMLStoreCodeLocation() As String
            Get
                Return String.Format("{0}\{1}", GetWorKSpaceFolder, "XmlStore.xml")
            End Get
        End Property
        Public Shared ReadOnly Property GetBinaryStoreCodeLocation() As String
            Get
                Return String.Format("{0}\{1}", GetWorKSpaceFolder, "BinaryStore.dat")
            End Get
        End Property

        Private Shared Sub CheckIfInitialized()
            If Not _initilized Then
                InitializeClass()
            End If
        End Sub
    End Class
End Namespace
