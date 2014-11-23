Namespace Compiler
    Public Class ResourceEntity
        Inherits EntityBase
        Private _resourceStoringType As String
        Public Property ResourceStoringType As String
            Get
                If IsNothing(_resourceStoringType) Then
                    Return "Byte()"
                Else
                    Return _resourceStoringType
                End If
            End Get
            Set(ByVal value As String)
                _resourceStoringType = value
            End Set
        End Property
    End Class
End Namespace
