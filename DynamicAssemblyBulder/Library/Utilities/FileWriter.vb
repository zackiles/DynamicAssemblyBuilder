Namespace Utilities
    Public Class FileWriter

        Public Shared FileQueue As List(Of String)
        Private Shared _internalManagedResources As List(Of String)
        Private Shared _internalEmbeddedResources As List(Of String)
        Sub New()
            _internalManagedResources = New List(Of String)
            _internalEmbeddedResources = New List(Of String)
            Dim ty As Type() = Reflection.Assembly.GetExecutingAssembly.GetTypes
            For Each ty1 As Type In ty
                Console.WriteLine(ty1.Namespace)
                If ty.GetType.Namespace = "My.Resources" Then

                End If
            Next
         
        End Sub
        Public Sub WriteFile(ByVal fileName)

        End Sub
    End Class
End Namespace
