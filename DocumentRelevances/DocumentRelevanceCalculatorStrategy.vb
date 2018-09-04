''' <summary>
''' Base class for document relevance calculation
''' </summary>
Public MustInherit Class DocumentRelevanceCalculatorStrategy
  ''' <summary>
  ''' Calculates the relevance for each of the documents,
  ''' using the summaries containing the necessary information for doing so
  ''' </summary>
  ''' <param name="documentSummaries">Collection of document summaries</param>
  ''' <returns>A map relating file name with its relevance</returns>
  Public MustOverride Function CalculateDocumentsRelevance(documentSummaries As List(Of DocumentSummary)) As Dictionary(Of String, Double)
End Class
