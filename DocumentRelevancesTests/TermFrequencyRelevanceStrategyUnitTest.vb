Imports System.Text
Imports DocumentRelevances
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TermFrequencyRelevanceStrategyUnitTest

  <TestMethod()> Public Sub GivenNoTermsReturnZeroRelevanceForDocuments()
    Dim terms As IEnumerable(Of String) = {}
    Dim tested = New TermFrequencyRelevanceStrategy()
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"bbb", 4}
      })
    }

    Dim result = tested.CalculateDocumentsRelevance(documentSummaries, terms)

    Assert.AreEqual(0.0, result("doc1"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentDoesNotContainTermReturnZeroRelevanceForDocument()
    Dim terms As IEnumerable(Of String) = {"aaa"}
    Dim tested = New TermFrequencyRelevanceStrategy()
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"bbb", 4}
      }),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"ccc", 3},
        {"bbb", 4}
      })
    }

    Dim result = tested.CalculateDocumentsRelevance(documentSummaries, terms)

    Assert.AreNotEqual(0.0, result("doc1"))
    Assert.AreEqual(0.0, result("doc2"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentsWithSameFrequencyReturnSameRelevanceForDocuments()
    Dim terms As IEnumerable(Of String) = {"aaa", "bbb"}
    Dim tested = New TermFrequencyRelevanceStrategy()
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 3},
        {"ccc", 4}
      }),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      })
    }

    Dim result = tested.CalculateDocumentsRelevance(documentSummaries, terms)

    Assert.IsTrue(result("doc1") = result("doc2"))
  End Sub

  <TestMethod()> Public Sub GivenDocumentsWithMoreFrequencyReturnMoreRelevanceForDocument()
    Dim terms As IEnumerable(Of String) = {"aaa", "bbb"}
    Dim tested = New TermFrequencyRelevanceStrategy()
    Dim documentSummaries = New List(Of DocumentSummary) From {
      New DocumentSummary("doc1", New Dictionary(Of String, Integer)() From {
        {"aaa", 5},
        {"ccc", 4}
      }),
      New DocumentSummary("doc2", New Dictionary(Of String, Integer)() From {
        {"bbb", 3},
        {"ccc", 4}
      })
    }

    Dim result = tested.CalculateDocumentsRelevance(documentSummaries, terms)

    Assert.IsTrue(result("doc1") > result("doc2"))
  End Sub

End Class