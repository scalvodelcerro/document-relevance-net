Public MustInherit Class DocumentRelevanceCalculatorStrategy
  Public MustOverride Function CalculateDocumentRelevance(documentSummaries As List(Of DocumentSummary)) As Dictionary(Of String, Double)
End Class
