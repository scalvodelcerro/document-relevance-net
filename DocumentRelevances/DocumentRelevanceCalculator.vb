﻿Imports System.IO

''' <summary>
''' Manager class for calculating the relevance of a set of documents
''' </summary>
Public Class DocumentRelevanceCalculator
  Implements IDisposable

  ''' <summary>
  ''' Defines the calculation strategy that will be used 
  ''' </summary>
  ''' <returns>The strategy</returns>
  Public Property Strategy As DocumentRelevanceCalculatorStrategy

  ''' <summary>
  ''' Terms of relevance for calculation
  ''' </summary>
  ''' <returns>A enumeration of terms of relevance</returns>
  Public Property Terms As IEnumerable(Of String)
    Get
      Return _terms
    End Get
    Set(value As IEnumerable(Of String))
      _terms = value.AsParallel().Select(Function(t) t.ToLower()).Distinct().ToList()
    End Set
  End Property
  Private _terms As IEnumerable(Of String) = New List(Of String)()

  ''' <summary>
  ''' Fired when the relevance of the processed documents is updated
  ''' </summary>
  Public Event DocumentRelevanceChanged()

  Private WithEvents Watcher As FileSystemWatcher
  Private documentSummaries As List(Of DocumentSummary)
  Private documentRelevances As Dictionary(Of String, Double)

  ''' <summary>
  ''' Constructor
  ''' </summary>
  Public Sub New()
    Me.documentSummaries = New List(Of DocumentSummary)()
  End Sub

  ''' <summary>
  ''' Sets the directory that contains the documents and starts listening for changes (new files)
  ''' in the directory
  ''' </summary>
  ''' <param name="directoryPath">Full path to the directory</param>
  Public Sub StartWatchingDirectory(directoryPath As String)
    ReadExistingFiles(directoryPath)
    Watcher = New FileSystemWatcher(directoryPath) With {
      .EnableRaisingEvents = True,
      .IncludeSubdirectories = False
    }
  End Sub

  ''' <summary>
  ''' Prints the n most relevant documents using the console
  ''' </summary>
  ''' <param name="limit">Number of most relevant documents to prinf</param>
  Public Sub PrintMostRelevantDocuments(limit As Integer)
    Console.WriteLine("--------------------------------------------------")
    Console.WriteLine("Documents processed: {0}", documentRelevances.Count)
    Console.WriteLine("  Showing top {0} most relevant documents,", limit)
    Console.WriteLine("  using [{0}] calculation", Strategy.Description)
    Console.WriteLine("  for the terms [{0}]", String.Join(", ", Terms))
    Console.WriteLine("--------------------------------------------------")
    For Each documentRelevance In documentRelevances.
      OrderByDescending(Function(x) x.Value).
      Take(limit).
      Select(Function(x, index) New With {.DocumentName = x.Key, .Relevance = x.Value, .RelevanceNumber = index + 1})

      Console.WriteLine("{0}-document: {1}, relevance: {2}",
                        documentRelevance.RelevanceNumber,
                        documentRelevance.DocumentName,
                        documentRelevance.Relevance)
    Next
    Console.WriteLine("--------------------------------------------------")
    Console.WriteLine()
  End Sub

  Private Sub ReadExistingFiles(directoryPath As String)
    Dim s = Stopwatch.StartNew()
    Directory.EnumerateFiles(directoryPath).AsParallel().
      ForAll(Sub(documentPath) documentSummaries.Add(CreateSummary(documentPath)))
    UpdateDocumentRelevances()
    s.Stop()
    Trace.WriteLine(String.Format("Process time: {0}", s.Elapsed))
  End Sub

  Private Sub UpdateDocumentRelevances()
    documentRelevances = Strategy.CalculateDocumentsRelevance(documentSummaries, Terms)
    RaiseEvent DocumentRelevanceChanged()
  End Sub

  Private Sub OnDocumentCreate(sender As Object, e As FileSystemEventArgs) Handles Watcher.Created
    documentSummaries.Add(CreateSummary(e.FullPath))
    UpdateDocumentRelevances()
  End Sub

  Private Sub OnDocumentDelete(sender As Object, e As FileSystemEventArgs) Handles Watcher.Deleted
    documentSummaries = documentSummaries.Where(Function(x) x.DocumentName <> e.Name).ToList()
    UpdateDocumentRelevances()
  End Sub

  Private Sub OnDocumentChange(sender As Object, e As FileSystemEventArgs) Handles Watcher.Changed
    documentSummaries = documentSummaries.Where(Function(x) x.DocumentName <> e.Name).ToList()
    documentSummaries.Add(CreateSummary(e.FullPath))
    UpdateDocumentRelevances()
  End Sub

  Private Sub OnDocumentRename(sender As Object, e As RenamedEventArgs) Handles Watcher.Renamed
    documentSummaries = documentSummaries.Where(Function(x) x.DocumentName <> e.OldName).ToList()
    documentSummaries.Add(CreateSummary(e.FullPath))
    UpdateDocumentRelevances()
  End Sub

  Private Function CreateSummary(documentPath As String) As DocumentSummary
    WaitReady(documentPath)
    Dim documentName = Path.GetFileName(documentPath)

    Dim termCounts As Dictionary(Of String, Integer) =
      File.ReadLines(documentPath).
        SelectMany(Function(line) line.Split()).
        GroupBy(Function(word) word.ToLower()).
        ToDictionary(Function(g) g.Key, Function(g) g.Count())

    Return New DocumentSummary(documentName, termCounts)
  End Function

#Region "IDisposable Support"
  Private disposedValue As Boolean

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not disposedValue Then
      If disposing Then
        Watcher.Dispose()
      End If
    End If
    disposedValue = True
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
  End Sub
#End Region
End Class
