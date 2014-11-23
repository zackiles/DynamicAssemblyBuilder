Imports System.Xml.Serialization
Imports System.Xml
Imports System.Text
Imports DynamicFramework.Application

Namespace Serializer
    Public Class Xml
        Inherits SerializaeBase
        Public Overrides Function DeSerialize(ByVal asType As Type, Optional ByVal fileName As String = Nothing) As Object
            If IsNothing(fileName) Then
                fileName = Settings.GetXMLStoreCodeLocation
            End If
            Dim deSerializer As New XmlSerializer(asType)
            Using outStream As IO.Stream = New IO.MemoryStream
                Using reader As XmlReader = XmlReader.Create(fileName)
                    Dim c1 As Compiler.AssemblyBackingStore = DirectCast(deSerializer.Deserialize(reader), Compiler.AssemblyBackingStore)
                    Return c1
                    outStream.Close()
                    reader.Close()
                End Using
            End Using
        End Function
        Public Overloads Function DeSerialize(ByVal asType As Type, ByVal stream As IO.Stream) As Object
            Dim deSerializer As New XmlSerializer(asType)
            Using outStream As IO.Stream = New IO.MemoryStream
                Using reader As XmlReader = XmlReader.Create(stream)
                    Dim c1 As Compiler.AssemblyBackingStore = DirectCast(deSerializer.Deserialize(reader), Compiler.AssemblyBackingStore)
                    Return c1
                    outStream.Close()
                    reader.Close()
                End Using
            End Using
        End Function

        Public Overrides Sub Serialize(ByVal objToSerialize As Object, Optional ByVal fileName As String = Nothing)
            If IsNothing(fileName) Then
                fileName = Settings.GetXMLStoreCodeLocation
            End If
            Dim serializer As New XmlSerializer(objToSerialize.GetType)
            Using outStream As IO.Stream = New IO.MemoryStream
                Using writer As XmlWriter = XmlWriter.Create(outStream, GetXmlWriterSettings())
                    serializer.Serialize(writer, objToSerialize)
                    Application.File.Write(outStream, fileName)
                    writer.Close()
                End Using
                outStream.Close()
            End Using
        End Sub
        Private Function GetXmlWriterSettings() As XmlWriterSettings
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = True
            settings.Encoding = Encoding.UTF8
            settings.CloseOutput = True
            settings.NewLineOnAttributes = True
            settings.NamespaceHandling = False
            settings.NewLineOnAttributes = True

            Return settings
        End Function
    End Class
End Namespace