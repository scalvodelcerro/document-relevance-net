Imports System.IO
Imports System.Threading

Public Module FileUtils
  Public Sub WaitReady(ByVal fileName As String)
    Dim maxTryNumber As Integer = 3
    Dim tries As Integer = 0
    While tries < maxTryNumber
      Try
        Using stream As Stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read Or FileShare.None)
          If stream IsNot Nothing Then
            Trace.WriteLine(String.Format("Output file {0} ready.", fileName))
            Exit While
          End If
        End Using
      Catch ex As FileNotFoundException
        Trace.WriteLine(String.Format("Output file {0} not yet ready ({1})", fileName, ex.Message))
      Catch ex As IOException
        Trace.WriteLine(String.Format("Output file {0} not yet ready ({1})", fileName, ex.Message))
      Catch ex As UnauthorizedAccessException
        Trace.WriteLine(String.Format("Output file {0} not yet ready ({1})", fileName, ex.Message))
      End Try
      tries += 1
      Thread.Sleep(500)
    End While
  End Sub
End Module
