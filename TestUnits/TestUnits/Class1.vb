Imports System
Imports System.IO
Imports System.Text
Imports System.Reflection
Imports DynamicFramework
Imports DynamicFramework.Application
Imports DynamicFramework.Compiler
Imports DynamicFramework.Decompiler
Imports System.Collections.Generic

Public Class Class1

    Const DebugStubName = "BasicStub"
    Public Shared Sub Main()
        Dim c1 As New Class1
        '   c1.BuildAssemblyBasic()
        'Console.WriteLine("Finished making serializaer")
        'c1.BuildAssemblyWithManagedResouceBasic()
        ''Console.SetWindowSize(Console.LargestWindowWidth - 20, Console.LargestWindowHeight)
        ''TestSaveBackingBinaryStore()
        ''Console.ReadLine()
        c1.DecompileAssemblyToIl()
        Logger.PrintAllEntriesToConsole()
        Console.ReadLine()
    End Sub
    'Public Sub TestLoadBackingBinaryStore()
    '    Compiler.BackingStoreProvider.LoadStore(Settings.GetBinaryStoreCodeLocation)
    'End Sub
    Public Sub TestSaveBackingBinaryStore()
        Compiler.BackingStoreProvider.CreateStoreBinary()
        Dim deserilazer As New Serializer.Binary

        Dim cone As Compiler.AssemblyBackingStore = DirectCast(deserilazer.DeSerialize(GetType(AssemblyBackingStore)), Compiler.AssemblyBackingStore)
        Console.WriteLine("Serilization results...")
        Console.WriteLine(cone.Optimized.ToString)
    End Sub


    'Public Sub TestLoadBackingStore()
    '    Compiler.BackingStoreProvider.LoadStore("")
    'End Sub
    Public Sub TestSaveBackingStore()
        Compiler.BackingStoreProvider.CreateStoreXml()
        Console.WriteLine("Serilization results...")
        '   Console.WriteLine(IO.File.ReadAllText(Settings.GetXMLStoreCodeLocation))
        Dim deserilazer As New Serializer.Xml

        Dim cone As Compiler.AssemblyBackingStore = DirectCast(deserilazer.DeSerialize(GetType(AssemblyBackingStore)), Compiler.AssemblyBackingStore)
        Console.WriteLine(cone.Optimized.ToString)
    End Sub
    Public Sub DecompileAssemblyToIl()
        Dim decompiler As New Decompiler.IlDasmAgent
        decompiler.AssemblyLocation = String.Format("{0}\{1}.exe", "C:\Users\Brittany\Documents\Visual Studio 2010\Projects\DynamicAssemblyBulder\StubGenerator\StubGenerator\bin\Debug\Output", "phanol")
        decompiler.OutputFileLocation = String.Format("{0}\{1}.il", Settings.GetWorKSpaceFolder, DebugStubName)
        decompiler.DecompileAssembly(IlDasmAgent.DecompileMode.FULL)
    End Sub
    Public Sub BuildAssemblyWithManagedResouceBasic()
        Dim assemblyDefinition As New Compiler.AssemblyDefinition
        Dim managedResourceProvider As New Compiler.ManagedResourceProvider
        Dim managedResourceEntity As New Compiler.ManagedResourceEntity
        Dim resourceEntity As New Compiler.ResourceEntity
        Dim sourceCodeEntity As New SourceCodeEntity
        Dim codeDomCompailer As New Compiler.CodeDomAgent
        resourceEntity.Location = Settings.GetWorKSpaceFolder + "/" + "test.xml"
        managedResourceEntity.ResourceInClassList.Add(resourceEntity)
        managedResourceEntity.RootNamespace = "Bender"
        managedResourceEntity.CreateStronglyTypedClass = True
        managedResourceProvider.CreateManagedResourcePackage(managedResourceEntity)
        Dim manaedResourceList As New List(Of ManagedResourceEntity)
        manaedResourceList.Add(managedResourceEntity)
        assemblyDefinition.ManagedResourceList = manaedResourceList
        sourceCodeEntity.Location = Settings.GetSourceCodeLocation("Bender")
        assemblyDefinition.AddSourceCode(sourceCodeEntity)
        assemblyDefinition.ShowConsoleWindow = True

        codeDomCompailer.GenerateAssembly(assemblyDefinition, True)
    End Sub
    Public Sub BuildAssemblyBasic()
        Dim assemblyDefinition As New Compiler.AssemblyDefinition
        Dim sourceFile As New SourceCodeEntity
        sourceFile.Location = Settings.GetSourceCodeLocation(DebugStubName)
        assemblyDefinition.AddSourceCode(sourceFile)
        Dim codeDomCompailer As New Compiler.CodeDomAgent
        codeDomCompailer.GenerateAssembly(assemblyDefinition, True)
    End Sub
    Public Sub BuildAssemblyFromIL()
        Dim assemblyDefinition As New Compiler.AssemblyDefinition

        Dim sourceFile As New SourceCodeEntity
        sourceFile.Location = String.Format("{0}\{1}.il", Settings.GetWorKSpaceFolder, DebugStubName)
        assemblyDefinition.AddSourceCode(sourceFile)
        Dim ilAsmCompailer As New Compiler.IlasmAgent
        assemblyDefinition.StrongSigningOn = True
        ilAsmCompailer.GenerateAssembly(assemblyDefinition)
    End Sub
  
End Class
