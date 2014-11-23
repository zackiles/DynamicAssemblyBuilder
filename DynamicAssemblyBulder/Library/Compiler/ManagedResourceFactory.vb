Imports DynamicFramework.Application
Namespace Compiler
    Public Class ManagedResourceFactory
        Public Shared Sub CreateResourceUnit(ByRef resourceEntity As ResourceEntity, ByRef writer As Resources.ResourceWriter)
            Select Case resourceEntity.ResourceStoringType
                Case "String"
                    CreateResourceUnitAsString(resourceEntity, writer)
                Case "Stream"
                    InternalException.ThrowException(InternalMessage.NotImplementedException, Nothing)
                Case "Byte()"
                    CreateResourceUnitAsByteArray(resourceEntity, writer)
                Case Else
                    InternalException.ThrowException(InternalMessage.NotImplementedException, Nothing)
            End Select
        End Sub
        Public Shared Sub CreateResourceUnitAsStream(ByRef resourceEntity As ResourceEntity, ByRef writer As Resources.ResourceWriter)
            ' Strongly typed class not being created when using a Stream for some reason. Need to investigate.
            'Try
            '    Dim fileStream As Object = New IO.FileStream(resourceEntity.Location, IO.FileMode.Open)
            '    writer.AddResource(IO.Path.GetFileName(resourceEntity.Location), fileStream)

            'Catch ex As Exception
            '    InternalException.ThrowException(ex)
            'End Try
            InternalException.ThrowException(InternalMessage.NotImplementedException, Nothing)
        End Sub
        Public Shared Sub CreateResourceUnitAsByteArray(ByRef resourceEntity As ResourceEntity, ByRef writer As Resources.ResourceWriter)
            writer.AddResource(IO.Path.GetFileName(resourceEntity.Location), resourceEntity.GetEntityAsObject)
        End Sub
        Public Shared Sub CreateResourceUnitAsString(ByRef resourceEntity As ResourceEntity, ByRef writer As Resources.ResourceWriter)
           writer.AddResource(IO.Path.GetFileName(resourceEntity.Location), resourceEntity.GetEntityAsObject)
        End Sub
    End Class
End Namespace
