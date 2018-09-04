Imports DocumentRelevances

Public Class TermFrequencyRelevanceStrategy
  Inherits DocumentRelevanceCalculatorStrategy

  Protected terms As IEnumerable(Of String)

  ''' <summary>
  ''' Constructor
  ''' </summary>
  ''' <param name="terms">Collection of terms of importance</param>
  Public Sub New(terms As IEnumerable(Of String))
    Me.terms = terms.Select(Function(term) term.ToLower()).Distinct().ToList()
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

  ''' <summary>
  ''' Calculates the frequency of a term in a document, proportional to the document size
  ''' </summary>
  ''' <param name="term">The term to search</param>
  ''' <param name="documentSummary">The document summary where the term is searcheds</param>
  ''' <returns>The relative frequency of the term</returns>
  Protected Function CalculateTfForTermInDocument(term As String, documentSummary As DocumentSummary) As Double
    Return documentSummary.GetFrequencyForTerm(term) / documentSummary.WordCount
  End Function
End Class
