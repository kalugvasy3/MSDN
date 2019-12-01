Public Class Exporgt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
        End If

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim strDownloadFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss")

        ' Your Function for Retrieving Data
        Dim DS As DataSet = RetrieveData()

        exportDataSetToExcel(Response, DS, strDownloadFileName)
        Response.End()

    End Sub

    Function RetrieveData() As DataSet
        'Below just for test purpose ...
        Dim ds As New DataSet
        Dim dt As New DataTable
        'DataTable - TEST...

        dt.TableName = "TEST 1"
        dt.Columns.Add("TestA")
        dt.Columns.Add("TestB")
        dt.Columns.Add("TestC")
        dt.Columns.Add("TestD,E")

        Dim dr As DataRow = dt.NewRow
        dr("TestA") = "A' "
        dr("TestB") = "B"" "
        dr("TestC") = "C<> "
        dr("TestD,E") = ",C, "

        dt.Rows.Add(dr)
        ds.Tables.Add(dt)

        Dim dt2 As New DataTable

        dt2.TableName = "TEST TEST 2"
        dt2.Columns.Add("TestA2")
        dt2.Columns.Add("TestB2")
        dt2.Columns.Add("TestC2")
        dt2.Columns.Add("TestD2,E2")

        Dim dr2 As DataRow = dt2.NewRow
        dr2("TestA2") = "A'2 "
        dr2("TestB2") = "B""2 "
        dr2("TestC2") = "C<>2 "
        dr2("TestD2,E2") = ",C,2 "

        dt2.Rows.Add(dr2)
        ds.Tables.Add(dt2)

        Return ds
    End Function

    'https://www.codeproject.com/Tips/19840/Export-to-Excel-using-VB-Net
    'https://en.wikipedia.org/wiki/Microsoft_Office_XML_formats
    'https://en.wikipedia.org/wiki/Microsoft_Excel
    'https://msdn.microsoft.com/en-us/library/aa140066(office.10).aspx

    Public Sub exportDataSetToExcel(ByVal respObj As HttpResponse, ByVal DS As DataSet, ByVal fileName As String)

        fileName = fileName + ".xml"

        Dim sb As New StringBuilder()

        sb.Append("<?xml version=""1.0"" encoding=""UTF-8""?> ")
        sb.AppendLine("<?mso-application progid=""Excel.Sheet""?> ")
        sb.AppendLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" ")
        sb.AppendLine("xmlns:x=""urn:schemas-microsoft-com:office:excel"" ")
        sb.AppendLine("xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" ")
        sb.AppendLine("xmlns:html=""http://www.w3.org/TR/REC-html40""> ")
        sb.AppendLine("")

        For Each tbl As System.Data.DataTable In DS.Tables

            Dim dc As System.Data.DataColumn
            Dim dr As System.Data.DataRow

            sb.AppendLine("<Worksheet ss:Name=""" + tbl.TableName + """>")
            sb.AppendLine("<Table>")

            ' see XML Spreadsheet Reference https://msdn.microsoft.com/en-us/library/aa140066(office.10).aspx
            sb.AppendLine("<Column ss:Index=""1"" ss:AutoFitWidth=""0"" ss:Width=""110""/>")

            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 0

            ' First Row as Column's Name
            sb.AppendLine("<Row>")
            For Each dc In tbl.Columns
                sb.AppendLine("<Cell><Data ss:Type=""String"">" + HttpUtility.HtmlEncode(dc.ColumnName.ToString) + "</Data></Cell>")
            Next
            sb.AppendLine("</Row>")

            ' Add each Row
            For Each dr In tbl.Rows
                sb.AppendLine("<Row>")
                For Each dc In tbl.Columns
                    sb.AppendLine("<Cell><Data ss:Type=""String"">" + HttpUtility.HtmlEncode(dr(dc.ColumnName).ToString) + "</Data></Cell>")
                Next
                sb.AppendLine("</Row>")
            Next

            sb.AppendLine("</Table>")
            sb.AppendLine("</Worksheet>")

        Next
        sb.AppendLine("</Workbook>")

        respObj.Clear()
        respObj.ClearHeaders()
        respObj.ContentType = "application/vnd.ms-excel"
        respObj.AppendHeader("content-disposition", "attachment; filename=" + fileName)

        Dim myData As Byte() = System.Text.Encoding.UTF8.GetBytes(sb.ToString)
        respObj.BinaryWrite(myData)  ' Binary data - see myData -  

    End Sub


End Class