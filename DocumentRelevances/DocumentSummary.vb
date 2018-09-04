Public Class DocumentSummary

  Public Sub New(documentName As String, termCounts As Dictionary(Of String, Integer), wordCount As Long)
    Me.DocumentName = documentName
    Me.TermCounts = termCounts
    Me.WordCount = wordCount
  End Sub

  Public ReadOnly Property DocumentName As String
  Public ReadOnly Property TermCounts As Dictionary(Of String, Integer)
  Public ReadOnly Property WordCount As Long

  Friend Function ContainsTerm(term As String) As Boolean
    Return TermCounts.Keys.Contains(term)
  End Function

  Friend Function GetFrequencyForTerm(term As String) As Integer
    Return If(ContainsTerm(term), TermCounts(term), 0)
  End Function
End Class

