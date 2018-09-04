''' <summary>
''' Summary of a document with the necessary information to perform relevance calculations
''' </summary>
Public Class DocumentSummary

  ''' <summary>
  ''' Constructor
  ''' </summary>
  ''' <param name="documentName">Name of the document</param>
  ''' <param name="termCounts">Map relating a term with its frequency</param>
  Public Sub New(documentName As String, termCounts As Dictionary(Of String, Integer))
    Me.DocumentName = documentName
    Me.TermCounts = termCounts
  End Sub

  Public ReadOnly Property DocumentName As String
  Public ReadOnly Property TermCounts As Dictionary(Of String, Integer)
  Public ReadOnly Property WordCount As Long
    Get
      Return TermCounts.Values.Sum()
    End Get
  End Property

  ''' <summary>
  ''' Checks if a term appears in the document
  ''' </summary>
  ''' <param name="term">Term to search in the document</param>
  ''' <returns>True if the document contains the term</returns>
  Public Function ContainsTerm(term As String) As Boolean
    Return TermCounts.Keys.Contains(term)
  End Function

  ''' <summary>
  ''' Gets the number of times that a term appears in the document
  ''' </summary>
  ''' <param name="term">Term to search in the document</param>
  ''' <returns>Frequency of the term</returns>
  Public Function GetFrequencyForTerm(term As String) As Integer
    Return If(ContainsTerm(term), TermCounts(term), 0)
  End Function
End Class

