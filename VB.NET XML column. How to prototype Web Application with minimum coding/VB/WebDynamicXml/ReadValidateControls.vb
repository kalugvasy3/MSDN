Imports System.Web.UI.Page
Imports System.Web.UI.WebControls


Public Class ReadValidateControls
    ''' <summary>
    ''' Build XML from ALL TexBox Controls on the Panel ...
    ''' </summary>
    ''' <param name="pnl"></param>
    ''' <param name="strErros"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetXMLStringFromAllTextBoxes(ByRef pnl As Panel, ByRef strErros As String) As String

        Dim strXML As String = ""

        Dim dt As New DataTable
        dt.TableName = "Record"


        'Base on Control ID logic build  DataTable
        'TextBox will be Used only

        For Each control As Control In pnl.Controls

            If control.[GetType]() = GetType(TextBox) Then
                Dim strColumnName As String = control.ID
                dt.Columns.Add(strColumnName)
            End If
        Next

        'Populate DataTable and Validate All Controls ID (Should not be empty)
        Dim dr As DataRow = dt.NewRow

        ' ColumnName is same like TextBox ID
        For Each column As DataColumn In dt.Columns
            Dim tb As TextBox = CType(pnl.FindControl(column.ColumnName), TextBox)
            Dim str As String = tb.Text
            dr(column.ColumnName) = str

            If str = "" Then
                tb.BackColor = Drawing.Color.LightYellow
                strErros += column.ColumnName + "\n"
            Else
                tb.BackColor = Drawing.Color.White
            End If
        Next

        dt.Rows.Add(dr)

        strXML = xmlConvertReadWrite.GetXMLString(dt)
        Return strXML

    End Function

End Class
