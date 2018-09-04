Imports System.IO

Public Class DocumentRelevanceCalculator
  Implements IDisposable
  Public Property Strategy As DocumentRelevanceCalculatorStrategy
  Public Event DocumentRelevanceChanged()

  Private WithEvents Watcher As FileSystemWatcher
  Private documentSummaries As List(Of DocumentSummary)
  Private documentRelevances As Dictionary(Of String, Double)

  Public Sub New()
    Me.documentSummaries = New List(Of DocumentSummary)()
  End Sub

  Public Sub ReadExistingFiles(directoryPath As String)
    Directory.EnumerateFiles(directoryPath).AsParallel().
      ForAll(Sub(documentPath)
               Dim documentSummary = CreateSummary(documentPath)
               documentSummaries.Add(documentSummary)
             End Sub)
    documentRelevances = Strategy.CalculateDocumentRelevance(documentSummaries)
    RaiseEvent DocumentRelevanceChanged()
  End Sub

  Public Sub StartWatchingDirectory(directoryPath As String)
    ReadExistingFiles(directoryPath)
    Watcher = New FileSystemWatcher(directoryPath)
  End Sub

  Public Sub PrintMostRelevantDocuments(limit As Integer)
    Console.WriteLine("--------------------------------------------------")
    For Each documentRelevance In documentRelevances.
      OrderByDescending(Function(x) x.Value).
      Take(limit).
      Select(Function(x) New With {.DocumentName = x.Key, .Relevance = x.Value})
      Console.WriteLine("Document: {0}, relevance: {1}", documentRelevance.DocumentName, documentRelevance.Relevance)
    Next
    Console.WriteLine("--------------------------------------------------")
    Console.WriteLine()
  End Sub

  Private Sub OnDocumentCreate(sender As Object, e As FileSystemEventArgs) Handles Watcher.Created
    Dim documentSummary = CreateSummary(e.FullPath)
    documentSummaries.Add(documentSummary)
    documentRelevances = Strategy.CalculateDocumentRelevance(documentSummaries)
    RaiseEvent DocumentRelevanceChanged()
  End Sub

  Private Function CreateSummary(documentPath As String) As DocumentSummary
    WaitReady(documentPath)
    Dim documentName = Path.GetFileName(documentPath)

    Dim words As IEnumerable(Of String) = File.ReadLines(documentPath).SelectMany(Function(line) line.Split())
    Dim wordCount = words.Count()

    Dim termCounts As Dictionary(Of String, Integer) = words.
  GroupBy(Function(word) word).
  ToDictionary(Function(g) g.Key, Function(g) g.Count())

    Return New DocumentSummary(documentName, termCounts, wordCount)
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
