Imports CommandLine
Imports DocumentRelevances

Module DocumentRelevancesMain
  Private calculator As DocumentRelevanceCalculator

  Class Options
    <[Option]("d"c, "directory", Required:=True, HelpText:="Directory containing the documents that will be processed")>
    Public Property Directory As String
    <[Option]("t"c, "terms", Required:=True, HelpText:="Terms with relevance in the documents")>
    Public Property Terms As IEnumerable(Of String)
    <[Option]("n"c, "topN", Required:=True, HelpText:="Top n results to be returned")>
    Public Property ResultsLimit As Integer
  End Class

  Sub Main(ByVal args As String())
    Parser.Default.ParseArguments(Of Options)(args).
      WithParsed(Sub(o As Options) RunProgram(o))
    Dim exitProgram As Boolean = False
    Do Until exitProgram
      Dim line As String = Console.ReadLine()
      If line.ToUpper().Equals("EXIT") Then exitProgram = True
    Loop
    calculator.Dispose()
  End Sub

  Private Sub RunProgram(o As Options)
    calculator = New DocumentRelevanceCalculator(o)
    calculator.ReadExistingFiles()
    calculator.PrintDocumentRelevances()
    calculator.StartWatchingDirectoryChanges()
  End Sub
End Module
