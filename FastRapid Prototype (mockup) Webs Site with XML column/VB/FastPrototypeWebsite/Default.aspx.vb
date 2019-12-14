Imports System.Data.Common
Public Class _Default
    Inherits System.Web.UI.Page

    Private dataComm As New CommonClassLibrary.DataCommon
    Private cmd As DbCommand = dataComm.commandDB
    Private mainControl As MainControl
    Private ph As ContentPlaceHolder


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsCallback Then
            LoadSearchGrid()
        End If
    End Sub


    'Load All rows 
    Protected Sub LoadSearchGrid()
        cmd.CommandText = "SELECT * FROM ""FastPrototypeWebSite"""
        Dim ds As DataSet = dataComm.GetDataSet(cmd)
        grdAll.DataSource = ds.Tables(0)
        grdAll.DataBind()
        Session("All") = ds.Tables(0)
    End Sub


    Protected Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        mainControl = CType(ph.FindControl("MainControl1"), MainControl)
        mainControl.XML = ""   ' Will be create new (empty) Session("CF") 
        Response.Redirect("Add_New.aspx")
    End Sub


    ' Code for grid All should be here - it is primary page which hold this controls...
    Protected Sub grdAll_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles grdAll.SelectedIndexChanging

        ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        ph.Visible = True
        mainControl = CType(ph.FindControl("MainControl1"), MainControl)

        Dim dt As DataTable = CType(Session("All"), DataTable)

        Session("selectedIndex") = e.NewSelectedIndex + grdAll.PageIndex * grdAll.PageSize
        Dim row As DataRow = dt.Rows(e.NewSelectedIndex + grdAll.PageIndex * grdAll.PageSize)
        mainControl.Visible = True
        mainControl.XML = row("XML").ToString
        ' pnlResult hold MainControl
        pnlResult.Visible = True

        lblCurrent.Text = "Current selected record #" + Session("selectedIndex").ToString + "   (PK_INDEX = " + CStr(row("PK_INDEX")) + ") "
    End Sub


    Protected Sub grdAll_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdAll.PageIndexChanging
        Dim dt As DataTable = CType(Session("All"), DataTable)
        pnlResult.Visible = False
        grdAll.DataSource = dt
        grdAll.SelectedIndex = -1
        grdAll.PageIndex = e.NewPageIndex
        grdAll.DataBind()
    End Sub


    Protected Sub grdAll_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdAll.RowDeleting

        ' SQLite syntax - you need to replace to your DB syntax ...
        Dim dt As DataTable = CType(Session("All"), DataTable)
        Dim row As DataRow = dt.Rows(e.RowIndex + grdAll.PageIndex * grdAll.PageSize)
        cmd.CommandText = "DELETE FROM ""FastPrototypeWebSite"" WHERE ""PK_INDEX"" = '" + row("PK_INDEX").ToString + "'"
        dataComm.ExecuteSqlNonQuery(cmd)

        LoadSearchGrid()
        pnlResult.Visible = False
    End Sub


    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ph = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        mainControl = CType(ph.FindControl("MainControl1"), MainControl)

        Dim dt As DataTable = CType(Session("All"), DataTable)
        Dim row As DataRow = dt.Rows(CInt(Session("selectedIndex")))

        Dim strError As String = mainControl.updateRow(CInt(row("PK_INDEX")))

        If strError = "" Then
            pnlResult.Visible = False
            LoadSearchGrid()
        Else
            Allert(strError)
        End If
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