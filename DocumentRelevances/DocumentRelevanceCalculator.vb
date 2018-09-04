Imports System.IO

Friend Class DocumentRelevanceCalculator
  Implements IDisposable
  Private options As Options
  Private documentSummaries As List(Of DocumentSummary)
  Private documentRelevances As Dictionary(Of String, Double)
  Private watcher As FileSystemWatcher

  Public Sub New(options As Options)
    Me.options = options
    Me.documentSummaries = New List(Of DocumentSummary)()
  End Sub

  Public Sub StartWatchingDirectoryChanges()
    watcher = New FileSystemWatcher(options.Directory)
    AddHandler watcher.Created, AddressOf OnDocumentCreate
    watcher.EnableRaisingEvents = True
  End Sub

  Private Sub OnDocumentCreate(sender As Object, e As FileSystemEventArgs)
    Dim documentSummary = CreateSummary(e.FullPath, options.Terms)
    documentSummaries.Add(documentSummary)
    CalculateDocumentRelevance()
    PrintDocumentRelevances()
  End Sub


  Private Function CreateSummary(documentPath As String, terms As IEnumerable(Of String)) As DocumentSummary
    WaitReady(documentPath)
    Dim documentName = Path.GetFileName(documentPath)

    Dim words As IEnumerable(Of String) = File.ReadLines(documentPath).SelectMany(Function(line) line.Split())
    Dim wordCount = words.Count()

    Dim termCounts As Dictionary(Of String, Integer) = words.
      Where(Function(word) terms.Contains(word)).
      GroupBy(Function(word) word).
      ToDictionary(Function(g) g.Key, Function(g) g.Count())

    Return New DocumentSummary(documentName, termCounts, wordCount)
  End Function

  Private Sub CalculateDocumentRelevance()
    documentRelevances = documentSummaries.ToDictionary(Function(documentSummary) documentSummary.DocumentName, Function(documentSummary) 0.0)
    For Each term As String In options.Terms
      Dim idf As Double = CalculateIdfForTerm(term)
      CType(documentSummaries.AsParallel(), ParallelQuery(Of DocumentSummary)).
            ForAll(Sub(documentSummary)
                     Dim tf As Double = CalculateTfForTermInDocument(term, documentSummary)
                     Dim tfIdf = tf * idf
                     documentRelevances(documentSummary.DocumentName) += tfIdf
                   End Sub)
    Next
  End Sub

  Private Sub PrintDocumentRelevances()
    Console.WriteLine("--------------------------------------------------")
    For Each documentRelevance In documentRelevances.
      OrderBy(Function(x) x.Value).
      Take(options.ResultsLimit).
      Select(Function(x) New With {.DocumentName = x.Key, .Relevance = x.Value})
      Console.WriteLine("Document: {0}, relevance: {1}", documentRelevance.DocumentName, documentRelevance.Relevance)
    Next
    Console.WriteLine("--------------------------------------------------")
    Console.WriteLine()
  End Sub
  Private Shared Function CalculateTfForTermInDocument(term As String, documentSummary As DocumentSummary) As Double
    Return documentSummary.GetFrequencyForTerm(term) / documentSummary.WordCount
  End Function

  Private Function CalculateIdfForTerm(term As String) As Double
    Dim documentCount = documentSummaries.Count
    Dim documentsContainingTerm = documentSummaries.Where(Function(document) document.ContainsTerm(term)).Count()
    Return Math.Log(documentCount / documentsContainingTerm)
  End Function

#Region "IDisposable Support"
  Private disposedValue As Boolean

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not disposedValue Then
      If disposing Then
        watcher.Dispose()
      End If
    End If
    disposedValue = True
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
  End Sub
#End Region
End Class
