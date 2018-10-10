''' <summary>
''' Base class for document relevance calculation
''' </summary>
Public MustInherit Class DocumentRelevanceCalculatorStrategy
  ''' <summary>
  ''' Description of the calculation algorythm
  ''' </summary>
  ''' <returns>Name of the calculation strategy</returns>
  Public MustOverride ReadOnly Property Description As String
  ''' <summary>
  ''' Calculates the relevance for each of the documents,
  ''' using the summaries containing the necessary information for doing so
  ''' </summary>
  ''' <param name="documentSummaries">Collection of document summaries</param>
  ''' <param name="terms">Enumerable of terms of relevance for the calculation</param>
  ''' 
  ''' <returns>A map relating file name with its relevance</returns>
  Public MustOverride Function CalculateDocumentsRelevance(
                                  documentSummaries As List(Of DocumentSummary),
                                  terms As IEnumerable(Of String)
                              ) As Dictionary(Of String, Double)
End Class
