Imports System.Text
Imports DocumentRelevances
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TfIdfRelevanceStrategyUnitTest

  <TestMethod()> Public Sub TestMethod1()
  End Sub

  <TestMethod()> Public Sub GivenNoTermsReturnZeroRelevanceForDocuments()
    Dim terms As IEnumerable(Of String) = {}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"bbb", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.AreEqual(0.0, result("doc1"))
  End Sub

  <TestMethod()> Public Sub GivenTermInAllDocumentsReturnZeroRelevanceForDocuments()
    Dim repeatedTerm = "aaa"
    Dim terms As IEnumerable(Of String) = {repeatedTerm}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {repeatedTerm, 3},
        {"bbb", 4}
      }, 7),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {repeatedTerm, 3},
        {"bbb", 4}
      }, 7),
      New DocumentSummary("doc3", New Dictionary(Of String, Integer)() From {
        {repeatedTerm, 3},
        {"bbb", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.AreEqual(0.0, result("doc1"))
    Assert.AreEqual(0.0, result("doc2"))
    Assert.AreEqual(0.0, result("doc3"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentDoesNotContainTermReturnZeroRelevanceForDocument()
    Dim terms As IEnumerable(Of String) = {"aaa"}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"bbb", 4}
      }, 7),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"ccc", 3},
        {"bbb", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.AreNotEqual(0.0, result("doc1"))
    Assert.AreEqual(0.0, result("doc2"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentsWithSameFrequencyReturnSameRelevanceForDocuments()
    Dim terms As IEnumerable(Of String) = {"aaa", "bbb"}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"ccc", 4}
      }, 7),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.IsTrue(result("doc1") = result("doc2"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentsWithMoreFrequencyReturnMoreRelevanceForDocument()
    Dim terms As IEnumerable(Of String) = {"aaa", "bbb"}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 5},
        {"ccc", 4}
      }, 7),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.IsTrue(result("doc1") > result("doc2"))
  End Sub

  <TestMethod()> Public Sub GivenTermInLessDocumentsReturnMoreRelevanceForTerm()
    Dim terms As IEnumerable(Of String) = {"aaa", "bbb"}
    Dim tested = New TfIdfRelevanceStrategy(terms)
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"ccc", 4}
      }, 7),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      }, 7),
      New DocumentSummary("doc3", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      }, 7)
    }

    Dim result = tested.CalculateDocumentRelevance(documentSummaries)

    Assert.IsTrue(result("doc1") > result("doc2"))
    Assert.IsTrue(result("doc1") > result("doc3"))
    Assert.IsTrue(result("doc2") = result("doc3"))
  End Sub

End Class