Imports DocumentRelevances

''' <summary>
''' Calculates the relevance of documents using tf-idf(term frequency-inverse document frequency) method.
''' Useful to give less importance to terms that appear in most documents, as they are less relevant.
''' </summary>
Public Class TfIdfRelevanceStrategy
  Inherits TermFrequencyRelevanceStrategy

  Public Overrides ReadOnly Property Description As String
    Get
      Return "Term Frequency\Inverse Document Frequency"
    End Get
  End Property

  Public Overrides Function CalculateDocumentsRelevance(
                              documentSummaries As List(Of DocumentSummary),
                              terms As IEnumerable(Of String)) As Dictionary(Of String, Double)
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

  ''' <summary>
  ''' Calculates the relevance of a term in a set of documents.
  ''' A term will be less relevant if it appears in the majority of the documents.
  ''' </summary>
  ''' <param name="term">The term</param>
  ''' <param name="documentSummaries">Collection of documents</param>
  ''' <returns>The inverse document frequency for the term</returns>
  Protected Function CalculateIdfForTerm(term As String, documentSummaries As List(Of DocumentSummary)) As Double
    Dim documentCount = documentSummaries.Count
    Dim documentsContainingTerm = documentSummaries.Where(Function(document) document.ContainsTerm(term)).Count()
    Return Math.Log(documentCount / documentsContainingTerm)
  End Function
End Class
