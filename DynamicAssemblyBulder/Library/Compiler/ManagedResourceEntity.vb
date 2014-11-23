Namespace Compiler
    Public Class ManagedResourceEntity
        Inherits EntityBase
        Private _resourceInClassList As List(Of ResourceEntity)
        Sub New()
            _resourceInClassList = New List(Of ResourceEntity)
        End Sub
        Public Property ResourceInClassList As List(Of ResourceEntity)
            Get
                Return _resourceInClassList
            End Get
            Set(ByVal resourceInClassList As List(Of ResourceEntity))
                _resourceInClassList = resourceInClassList
            End Set
        End Property
        Private _resourceExtension As String
        Public Property ResourceExetension As String
            Get
                Return _resourceExtension
            End Get
            Set(ByVal resExtension As String)
                _resourceExtension = resExtension
            End Set
        End Property
        Private _stronglyTypeClassName As String
        Public Property StronglyTypedClassName As String
            Get
                If IsNothing(_stronglyTypeClassName) Then
                    Return Name
                Else
                    Return _stronglyTypeClassName
                End If
            End Get
            Set(ByVal value As String)
                _stronglyTypeClassName = value
            End Set
        End Property
        Public ReadOnly Property StronglyTypedClassLocation As String
            Get
                Return String.Format("{0}\{1}.vb", Application.Settings.GetWorKSpaceFolder, StronglyTypedClassName)
            End Get
        End Property
        Private _createStronglyTypedClass As Boolean
        Public Property CreateStronglyTypedClass As Boolean
            Get
                Return _createStronglyTypedClass
            End Get
            Set(ByVal value As Boolean)
                _createStronglyTypedClass = value
            End Set
        End Property
      
    End Class
End Namespace