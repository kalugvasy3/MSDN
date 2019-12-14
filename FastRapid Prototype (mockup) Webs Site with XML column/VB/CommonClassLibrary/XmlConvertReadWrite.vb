Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text

'Some code was built base on below links...
'https://msdn.microsoft.com/en-us/library/microsoft.visualbasic.fileio.textfieldparser(v=vs.110).aspx
'http://stackoverflow.com/questions/16606753/populating-a-dataset-from-a-csv-file
'http://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable

'You have not to work directly with XML column...
'You MUST import/export XML file to/from DataSet ...

Public Class XmlConvertReadWrite

    ''' <summary>
    ''' Wrap DataSet to XML tag and convert to XML
    ''' </summary>
    ''' <param name="dtTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetXMLString(ByVal dtTable As DataTable) As String

        Dim strXML As String = ""
        Dim ds As DataSet = New DataSet
        ds.DataSetName = "XML"  ' General Name for DataSet
        ds.Tables.Add(dtTable)

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
    ''' Write DataSet to XML string
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetXMLString(ByVal ds As DataSet) As String
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
    ''' XML string to DataSet
    ''' </summary>
    ''' <param name="strXml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSetFromXml(ByVal strXml As String) As DataSet

        Dim ds As DataSet = New DataSet

        Try
            ds.ReadXml(New MemoryStream(Encoding.UTF8.GetBytes(strXml)))
        Catch ex As Exception
            Throw
        End Try

        Return ds
    End Function


    ''' <summary>
    ''' Automatically Convert Column XML to new DataSet 
    ''' XML can include multiple tables.
    ''' Assign fro each table PK_INDEX
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSetFromXmlColumn(ByVal dt As DataTable) As DataSet

        Dim dsOut As New DataSet
        dsOut.DataSetName = "XML"

        If dt.Rows.Count = 0 Then
            Return Nothing
        End If

        ' DataTable can include multiple rows with different PK. (PK_INDEX is column in "dt")

        For Each dr In dt.Rows
            'Create TEMP DataSet
            Dim dsTemp As New DataSet
            'Set Name - XML
            dsTemp.DataSetName = "XML"
            'Bring Column "XML" and read to dsTemp
            Dim strXML As String = dr("XML") ' DataTable has column XML
            dsTemp.ReadXml(New MemoryStream(Encoding.UTF8.GetBytes(strXML)))

            'For each tables (XML can include multiple tables) add PK_INDEX
            For Each t As DataTable In dsTemp.Tables

                Dim col = New DataColumn

                col.DataType = System.Type.GetType("System.String")
                col.ColumnName = "PK_INDEX"                          ' See First Run page ()

                t.Columns.Add(col)

                For Each row In t.Rows
                    row.Item("PK_INDEX") = dr("PK_INDEX")
                Next
            Next
            'Merge TEMP table with dsOut
            'AcceptChanges !!!
            dsOut.Merge(dsTemp)
            dsOut.AcceptChanges()
            'Repeat fro each row in "dt"
        Next

        Return dsOut

    End Function


    Public Function GetDataTableFromCsvString(csvBody As String, isHeadings As Boolean) As DataTable
        Dim MethodResult As DataTable = Nothing
        Try
            Dim MemoryStream As New MemoryStream()
            Dim StreamWriter As New StreamWriter(MemoryStream)

            StreamWriter.Write(csvBody)
            StreamWriter.Flush()
            MemoryStream.Position = 0

            Using TextFieldParser As New TextFieldParser(MemoryStream)
                If isHeadings Then
                    MethodResult = GetDataTableFromTextFieldParser(TextFieldParser)
                Else
                    MethodResult = GetDataTableFromTextFieldParserNoHeadings(TextFieldParser)
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try
        Return MethodResult
    End Function


    Private Function GetDataTableFromTextFieldParser(textFieldParser As TextFieldParser) As DataTable
        Dim MethodResult As DataTable = Nothing
        Try
            textFieldParser.SetDelimiters(New String() {","})
            textFieldParser.HasFieldsEnclosedInQuotes = True

            Dim ColumnFields As String() = textFieldParser.ReadFields()
            Dim dt As New DataTable()
            ' Column Name should not include space
            For Each ColumnField As String In ColumnFields
                Dim DataColumn As New DataColumn(ColumnField)
                DataColumn.AllowDBNull = True
                dt.Columns.Add(DataColumn)
            Next


            While Not textFieldParser.EndOfData
                Dim Fields As String() = textFieldParser.ReadFields()
                For i As Integer = 0 To Fields.Length - 1
                    If Fields(i) = "" Then
                        Fields(i) = Nothing
                    End If
                Next

                dt.Rows.Add(Fields)
            End While
            MethodResult = dt
        Catch ex As Exception
            Throw
        End Try
        Return MethodResult
    End Function


    Private Function GetDataTableFromTextFieldParserNoHeadings(textFieldParser As TextFieldParser) As DataTable
        Dim MethodResult As DataTable = Nothing
        Try
            textFieldParser.SetDelimiters(New String() {","})
            textFieldParser.HasFieldsEnclosedInQuotes = True

            Dim FirstPass As Boolean = True
            Dim dt As New DataTable()

            While Not textFieldParser.EndOfData
                Dim Fields As String() = textFieldParser.ReadFields()
                If FirstPass Then
                    For i As Integer = 0 To Fields.Length - 1
                        Dim DataColumn As New DataColumn("Column " + i)
                        DataColumn.AllowDBNull = True
                        dt.Columns.Add(DataColumn)
                    Next
                    FirstPass = False
                End If

                For i As Integer = 0 To Fields.Length - 1
                    If Fields(i) = "" Then
                        Fields(i) = Nothing
                    End If
                Next
                dt.Rows.Add(Fields)
            End While
            MethodResult = dt
        Catch ex As Exception
            Throw
        End Try
        Return MethodResult
    End Function



End Class
