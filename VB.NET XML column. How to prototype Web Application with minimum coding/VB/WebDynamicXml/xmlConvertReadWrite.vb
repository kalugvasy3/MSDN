Imports Microsoft.VisualBasic.FileIO
Imports System.IO

'You have not to work directly with XML column...
'You MUST import/export XML file to/from DataSet ...

Public Class xmlConvertReadWrite

    ''' <summary>
    ''' One Row Table. Assign DataSet name like XML, add your table, write to XML string.
    ''' </summary>
    ''' <param name="dtOneRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetXMLString(ByVal dtOneRow As DataTable) As String

        Dim strXML As String
        Dim ds As DataSet = New DataSet
        ds.DataSetName = "XML"  ' General Name for DataSet
        ds.Tables.Add(dtOneRow)

        Try
            Dim writer As New System.IO.StringWriter
            ds.WriteXml(writer)
            strXML = writer.ToString
        Catch ex As Exception
            Throw
        End Try
        Return strXML
    End Function

    ''' <summary>
    ''' XML string from DataSet. Multiply Tables, Records...
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetXMLString(ByVal ds As DataSet) As String
        Dim strXML As String
        Try
            Dim writer As New System.IO.StringWriter
            ds.WriteXml(writer)
            strXML = writer.ToString
        Catch ex As Exception
            Throw
        End Try
        Return strXML
    End Function

    ''' <summary>
    ''' Read XML file to DataSet
    ''' </summary>
    ''' <param name="strXml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDataSetFromXml(ByVal strXml As String) As DataSet

        Dim ds As DataSet = New DataSet

        Try
            ds.ReadXml(New MemoryStream(Encoding.UTF8.GetBytes(strXml)))
        Catch ex As Exception
            Throw
        End Try

        Return ds
    End Function

  

End Class
