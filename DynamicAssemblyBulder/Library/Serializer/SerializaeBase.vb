Namespace Serializer
    Public MustInherit Class SerializaeBase
        MustOverride Function DeSerialize(ByVal asType As Type, Optional ByVal fileName As String = Nothing) As Object
        MustOverride Sub Serialize(ByVal objToSerialize As Object, Optional ByVal fileName As String = Nothing)
    End Class
End Namespace