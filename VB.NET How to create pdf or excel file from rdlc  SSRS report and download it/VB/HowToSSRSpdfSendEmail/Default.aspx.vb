Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Dim dtA As New DataTable

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click

        Session("dtA") = dtA
        Session("Email") = txtEmail.Text

        Response.Redirect("~/Reports/ReportSSRS.aspx")
    End Sub
End Class