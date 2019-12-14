' This page help create data table which store your data. 
' Run just once.
' For test purpose I used SQLite.
' Please install SQLite provider first.

'Add SQLite to DbProviderFactories - DataCommon use DbProviderFactories-->
'Download needed package from here  https://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki 
'For example Precompiled Binaries for 64-bit Windows (.NET Framework 4.0) https://system.data.sqlite.org/downloads/1.0.105.2/sqlite-netFx40-binary-bundle-x64-2010-1.0.105.2.zip




Imports System.Text

Public Class FirstRun
    Inherits System.Web.UI.Page

    Dim strInstraction As String = "Select Right Code and [Create] ... or enter code yourself - with columns [PK_INDEX], [XML] ..."

    Dim strMsSql As String = "-- CREATE TABLE ""FastPrototypeWebSite""(""PK_INDEX"" int IDENTITY(1,1) PRIMARY KEY , ""XML""  XML    NOT NULL,  ""DATE"" DATE   NOT NULL) ; "
    Dim strSqLite As String = "   CREATE TABLE ""FastPrototypeWebSite"" (""PK_INDEX"" int NOT NULL, ""XML"" TEXT NOT NULL,""DATE"" TEXT, PRIMARY KEY( ""PK_INDEX"")); "
    Dim strOracle As String = "-- CREATE TABLE ""FastPrototypeWebSite""(""PK_INDEX"" int IDENTITY(1,1) PRIMARY KEY , ""XML""  XML    NOT NULL,  ""DATE"" DATE   NOT NULL); "
    Dim strDB2 As String = "-- CREATE TABLE ""FastPrototypeWebSite""(""PK_INDEX"" int IDENTITY(1,1) PRIMARY KEY , ""XML""  XML    NOT NULL,  ""DATE"" DATE   NOT NULL); "


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            txtCreateTable.Text = "-- Comment/UnComment/Edit  below TEXT and [Create], You should have a privilege to create the table..." + Environment.NewLine
            txtCreateTable.Text += strMsSql + Environment.NewLine
            txtCreateTable.Text += strSqLite + Environment.NewLine
            txtCreateTable.Text += strOracle + Environment.NewLine
            txtCreateTable.Text += strDB2 + Environment.NewLine

        End If
    End Sub

    Protected Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim dc As New CommonClassLibrary.DataCommon ' it create connection to DataBase -- see web.config
        Dim com = dc.commandDB
        com.CommandText = txtCreateTable.Text
        Try
            dc.ExecuteSqlNonQuery(com)
            Allert("DataBase and Table were created. \nChange Start page to Default.aspx!")
        Catch ex As Exception
            Allert(ex.Message.Replace("'", "''").Replace(Environment.NewLine, "\n"))
        End Try

    End Sub

    Protected Sub Allert(ByVal message As String)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

End Class