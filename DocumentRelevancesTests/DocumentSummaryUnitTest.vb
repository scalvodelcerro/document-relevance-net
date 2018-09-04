Imports System.Text
Imports DocumentRelevances
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class DocumentSummaryUnitTest

  <TestMethod()> Public Sub GivenEmptyDocumentReturnContainsTermFalse()
    Dim tested = New DocumentSummary("doc1", New Dictionary(Of String, Integer)(), 0)

    Assert.IsFalse(tested.ContainsTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnFalseIfDoesNotContainTerm()
    Dim tested = New DocumentSummary("doc1",
                                     New Dictionary(Of String, Integer)() From {
                                        {"term1", 3},
                                        {"term2", 4}
                                     }, 0)

    Assert.IsFalse(tested.ContainsTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnTrueIfContainsTerm()
    Dim tested = New DocumentSummary("doc1",
                                     New Dictionary(Of String, Integer)() From {
                                        {"term1", 3},
                                        {"term2", 4}
                                     }, 0)

    Assert.IsTrue(tested.ContainsTerm("term1"))
  End Sub

  <TestMethod()> Public Sub GivenEmptyDocumentReturnZeroFrequency()
    Dim tested = New DocumentSummary("doc1", New Dictionary(Of String, Integer)(), 0)

    Assert.AreEqual(0, tested.GetFrequencyForTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnZeroFrequencyIfDoesNotContainTerm()
    Dim tested = New DocumentSummary("doc1",
                                     New Dictionary(Of String, Integer)() From {
                                        {"term1", 3},
                                        {"term2", 4}
                                     }, 0)

    Assert.AreEqual(0, tested.GetFrequencyForTerm("whatever"))
  End Sub

  <TestMethod()> Public Sub GivenNonEmptyDocumentReturnFrequencies()
    Dim term1Name = "term1", term2Name = "term2"
    Dim term1Count = 3, term2Count = 4

    Dim tested = New DocumentSummary("doc1",
                                     New Dictionary(Of String, Integer)() From {
                                        {term1Name, term1Count},
                                        {term2Name, term2Count}
                                     }, 0)

    Assert.AreEqual(term1Count, tested.GetFrequencyForTerm(term1Name))
    Assert.AreEqual(term2Count, tested.GetFrequencyForTerm(term2Name))
  End Sub

End Class