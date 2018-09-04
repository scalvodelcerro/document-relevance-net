Imports DocumentRelevances

Public Class TermFrequencyRelevanceStrategy
  Inherits DocumentRelevanceCalculatorStrategy

  Private terms As IEnumerable(Of String)

  Public Sub New(terms As IEnumerable(Of String))
    Me.terms = terms
  End Sub

  Public Overrides Function CalculateDocumentsRelevance(documentSummaries As List(Of DocumentSummary)) As Dictionary(Of String, Double)
    Dim documentRelevances = documentSummaries.ToDictionary(Function(documentSummary) documentSummary.DocumentName, Function(documentSummary) 0.0)
    For Each term As String In terms
      CType(documentSummaries.AsParallel(), ParallelQuery(Of DocumentSummary)).
            ForAll(Sub(documentSummary)
                     Dim tf As Double = CalculateTfForTermInDocument(term, documentSummary)
                     documentRelevances(documentSummary.DocumentName) += tf
                   End Sub)
    Next
    Return documentRelevances
  End Function

  Protected Function CalculateTfForTermInDocument(term As String, documentSummary As DocumentSummary) As Double
    Return documentSummary.GetFrequencyForTerm(term) / documentSummary.WordCount
  End Function
End Class
