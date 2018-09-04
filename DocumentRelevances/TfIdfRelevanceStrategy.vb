Imports DocumentRelevances

''' <summary>
''' Calculates the relevance of documents using tf-idf(term frequency-inverse document frequency) method.
''' Useful to give less importance to terms that appear in most documents, as they are less relevant.
''' </summary>
Public Class TfIdfRelevanceStrategy
  Inherits DocumentRelevanceCalculatorStrategy
  Private terms As IEnumerable(Of String)

  ''' <summary>
  ''' Constructor
  ''' </summary>
  ''' <param name="terms">Collection of terms of importance</param>
  Public Sub New(terms As IEnumerable(Of String))
    Me.terms = terms
  End Sub

  Public Overrides Function CalculateDocumentRelevance(documentSummaries As List(Of DocumentSummary)) As Dictionary(Of String, Double)
    Dim documentRelevances = documentSummaries.ToDictionary(Function(documentSummary) documentSummary.DocumentName, Function(documentSummary) 0.0)
    For Each term As String In terms
      Dim idf As Double = CalculateIdfForTerm(term, documentSummaries)
      CType(documentSummaries.AsParallel(), ParallelQuery(Of DocumentSummary)).
            ForAll(Sub(documentSummary)
                     Dim tf As Double = CalculateTfForTermInDocument(term, documentSummary)
                     Dim tfIdf = tf * idf
                     documentRelevances(documentSummary.DocumentName) += tfIdf
                   End Sub)
    Next
    Return documentRelevances
  End Function

  Private Function CalculateTfForTermInDocument(term As String, documentSummary As DocumentSummary) As Double
    Return documentSummary.GetFrequencyForTerm(term) / documentSummary.WordCount
  End Function

  Private Function CalculateIdfForTerm(term As String, documentSummaries As List(Of DocumentSummary)) As Double
    Dim documentCount = documentSummaries.Count
    Dim documentsContainingTerm = documentSummaries.Where(Function(document) document.ContainsTerm(term)).Count()
    Return Math.Log(documentCount / documentsContainingTerm)
  End Function
End Class
