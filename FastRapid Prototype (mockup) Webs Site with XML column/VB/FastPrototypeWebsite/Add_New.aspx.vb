'Please see page FirstRun.aspx - which run at once .... 
' For test purpose I used SQLite.
' Please install SQLite provider first.

'Add SQLite to DbProviderFactories - DataCommon use DbProviderFactories-->
'Download needed package from here  https://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki 
'For example Precompiled Binaries for 64-bit Windows (.NET Framework 4.0) https://system.data.sqlite.org/downloads/1.0.105.2/sqlite-netFx40-binary-bundle-x64-2010-1.0.105.2.zip

Imports System.Data.Common

'https://msdn.microsoft.com/en-us/library/bb398780.aspx


Public Class Add_New
    Inherits System.Web.UI.Page

    ' Private dataComm As New CommonClassLibrary.DataCommon
    ' Private cmd As DbCommand = dataComm.commandDB
    Private cf As New CommonClassLibrary.CommonFunction
    Private mainControl As MainControl
    Private ph As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
            'mainControl = CType(ph.FindControl("MainControl1"), MainControl)
        End If
    End Sub


    Protected Sub btnResetAll_Click(sender As Object, e As EventArgs) Handles btnResetAll.Click
        ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        mainControl = CType(ph.FindControl("MainControl1"), MainControl)
        cf.resetAllControls(mainControl.Controls)
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        mainControl = CType(ph.FindControl("MainControl1"), MainControl)

        Dim strError As String = mainControl.insertNewRow()

        If strError = "" Then
            Response.Redirect("Default.aspx")
        Else
            Allert(strError)
        End If


    End Sub


    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Response.Redirect("Default.aspx")
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