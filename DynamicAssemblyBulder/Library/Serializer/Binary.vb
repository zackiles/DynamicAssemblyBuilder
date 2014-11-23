Imports DynamicFramework.Application
Imports System.IO
Imports System.Runtime.Serialization

Namespace Serializer
    Public Class Binary
        Inherits SerializaeBase

        Public Overrides Function DeSerialize(ByVal asType As Type, Optional ByVal fileName As String = Nothing) As Object
            If IsNothing(fileName) Then
                fileName = Settings.GetBinaryStoreCodeLocation
            End If
            Dim fmt As Formatters.Binary.BinaryFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter
            fmt.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            Dim stm As IO.Stream = New IO.FileStream(fileName, FileMode.Open)
            Dim returnObject As Object = DirectCast(fmt.Deserialize(stm), Compiler.AssemblyBackingStore)
            stm.Close()
            stm.Dispose()
            Return returnObject
        End Function

        Public Overrides Sub Serialize(ByVal objToSerialize As Object, Optional ByVal fileName As String = Nothing)
            If IsNothing(fileName) Then
                fileName = Settings.GetBinaryStoreCodeLocation
            End If
            Dim fmt As Runtime.Serialization.Formatters.Binary.BinaryFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter
            fmt.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            Dim fs As New IO.FileStream(fileName, FileMode.Create)
            fmt.Serialize(fs, New Compiler.AssemblyBackingStore)
            fs.Close()
            fs.Dispose()

        End Sub
    End Class
End Namespace