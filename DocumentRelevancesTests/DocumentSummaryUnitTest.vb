Imports System.Text
Imports DocumentRelevances
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class DocumentSummaryUnitTest

  <TestMethod()> Public Sub GivenEmptyDocumentReturnContainsTermFalse()
    Dim tested = CreateEmptySummary()

    Assert.IsFalse(tested.ContainsTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnFalseIfDoesNotContainTerm()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested As DocumentSummary = CreateNonEmptySummary(term1Name, term2Name, term1Count, term2Count)

    Assert.IsFalse(tested.ContainsTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnTrueIfContainsTerm()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested As DocumentSummary = CreateNonEmptySummary(term1Name, term2Name, term1Count, term2Count)

    Assert.IsTrue(tested.ContainsTerm("term1"))
  End Sub

  <TestMethod()> Public Sub GivenEmptyDocumentReturnZeroFrequency()
    Dim tested = CreateEmptySummary()

    Assert.AreEqual(0, tested.GetFrequencyForTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnZeroFrequencyIfDoesNotContainTerm()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested As DocumentSummary = CreateNonEmptySummary(term1Name, term2Name, term1Count, term2Count)

    Assert.AreEqual(0, tested.GetFrequencyForTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnFrequencies()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested As DocumentSummary = CreateNonEmptySummary(term1Name, term2Name, term1Count, term2Count)

    Assert.AreEqual(term1Count, tested.GetFrequencyForTerm(term1Name))
    Assert.AreEqual(term2Count, tested.GetFrequencyForTerm(term2Name))
  End Sub

  <TestMethod()> Public Sub GivenEmptyDocumentReturnZeroWordCount()
    Dim tested As DocumentSummary = CreateEmptySummary()

    Assert.AreEqual(0L, tested.WordCount)
  End Sub


  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnSumOfCountsAsWordCount()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested As DocumentSummary = CreateNonEmptySummary(term1Name, term2Name, term1Count, term2Count)

    Assert.AreEqual(Convert.ToInt64(term1Count + term2Count), tested.WordCount)
  End Sub

  Private Shared Function CreateEmptySummary() As DocumentSummary
    Return New DocumentSummary("doc1", New Dictionary(Of String, Integer)())
  End Function

  Private Shared Function CreateNonEmptySummary(term1Name As String, term2Name As String, term1Count As Integer, term2Count As Integer) As DocumentSummary
    Return New DocumentSummary("doc1",
                                New Dictionary(Of String, Integer)() From {
                                  {term1Name, term1Count},
                                  {term2Name, term2Count}
                                })
  End Function
End Class