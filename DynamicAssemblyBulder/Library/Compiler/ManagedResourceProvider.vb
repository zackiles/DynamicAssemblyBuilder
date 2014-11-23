Imports DynamicFramework.Application
Namespace Compiler
    Public Class ManagedResourceProvider
        Private _resGenProvider As ShellProcessAgent
        Private _resWriter As Resources.ResourceWriter
        Private _mainResourceName As String
        Private _resGenFirstOptions As String
        Private Const _DefaultClassName As String = "Resources"
        Public Sub New()
            _resGenProvider = New ShellProcessAgent
            _resGenProvider.ProcessLocation = Settings.GetResGenLocation
        End Sub
        Public Sub CreateManagedResourcePackage(ByRef managedResourceEntity As ManagedResourceEntity)
            ValidateInput(managedResourceEntity)
            Try
                _mainResourceName = String.Format("{0}\{1}.resources", Settings.GetWorKSpaceFolder, managedResourceEntity.RootNamespace + ".Resources")

                _resWriter = New Resources.ResourceWriter(_mainResourceName)
                For Each resourceItem As ResourceEntity In managedResourceEntity.ResourceInClassList
                    ManagedResourceFactory.CreateResourceUnit(resourceItem, _resWriter)
                Next
                FinalizeResource(managedResourceEntity)
                ConfigureResourcePackage(managedResourceEntity)
            Catch ex As Exception
                InternalException.ThrowException(ex)
            End Try
        End Sub
        Private Sub ValidateInput(ByRef managedResourceEntity As ManagedResourceEntity)
            If IsNothing(managedResourceEntity.RootNamespace) Then
                InternalException.ThrowException("ERROR! No MainNameSpace in resource model, cannot generate resource.", New Object() {Me})
            End If
            If IsNothing(managedResourceEntity.RootNamespace) Then
                InternalException.ThrowException("ERROR! No Resoucres have been added to the list, cannot generate resource.", New Object() {Me})
            End If
        End Sub
        Private Sub ConfigureResourcePackage(ByRef managedResourceEntity As ManagedResourceEntity)
            If managedResourceEntity.CreateStronglyTypedClass = True Then
                If managedResourceEntity.StronglyTypedClassName Is Nothing Then
                    Logger.AddToLog("CreateStronglyTypedClass is activated, but no classname chosen.")
                    Logger.AddToLog(String.Format("Reverting to default strongly typed class name - ""{0}"".", _DefaultClassName))
                    managedResourceEntity.StronglyTypedClassName = managedResourceEntity.RootNamespace + "." + _DefaultClassName
                    managedResourceEntity.Name = _DefaultClassName
                End If
                CreateStronglyTypedClass(managedResourceEntity)
            End If
        End Sub
        Private Sub FinalizeResource(ByRef managedResourceEntity As ManagedResourceEntity)
            _resWriter.Generate()
            '    _resWriter.Close()
            _resWriter.Dispose()
        End Sub
        Private Sub CreateStronglyTypedClass(ByRef managedResourceEntity As ManagedResourceEntity)

            _resGenFirstOptions = Chr(34) + IO.Path.GetFileName(_mainResourceName) + Chr(34) + String.Format(" /str:vb,{0},{1},""{2}""", _
                                  managedResourceEntity.RootNamespace, "Resources", IO.Path.GetFileName(managedResourceEntity.StronglyTypedClassLocation))

            _resGenProvider.StartShell(_resGenFirstOptions)
        End Sub
    End Class
End Namespace
