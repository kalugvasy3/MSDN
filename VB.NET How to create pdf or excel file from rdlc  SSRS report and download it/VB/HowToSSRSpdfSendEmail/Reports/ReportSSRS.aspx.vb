Imports System.Net.Mail
Imports System.IO

Public Class ReportSSRS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadReport()
        Else
        End If

    End Sub


    Private Sub LoadReport()


        'Dim dtA As DataTable = CType(Session("dtA"), DataTable)

        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing
        Dim streamIds As String() = Nothing
        Dim mimeType As String = String.Empty
        Dim encoding As String = String.Empty
        Dim extension As String = String.Empty
        Dim filenameExtension As String = String.Empty

        Dim reportPath As String = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Reports\TestReport.rdlc"

        '//Create MS Report object
        rv.LocalReport.ReportPath = reportPath
        rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
        rv.LocalReport.SetBasePermissionsForSandboxAppDomain(New System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted))

        rv.LocalReport.DataSources.Clear()   ' MUST CLEAR DATA 
        'rv.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("dtA", dtA))

        Dim param As List(Of Microsoft.Reporting.WebForms.ReportParameter) = New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        'param.Add(New Microsoft.Reporting.WebForms.ReportParameter("pReportNameA", reportNameA))

        rv.LocalReport.SetParameters(param)
        rv.AsyncRendering = True
        rv.DataBind()
        rv.LocalReport.Refresh()

        ' ----------------------------------------------------- Open in web page
        Dim bytes As Byte() = rv.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamIds, warnings)

        Response.Clear()
        Response.ContentType = "application/pdf"

        ' ----------------------------------------------------- Download like PDF

        ' OR

        'Dim bytes As Byte() = rv.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamIds, warnings)
        'Response.Clear()
        'Response.ContentType = "application/pdf"
        'Response.AppendHeader("Content-Disposition", "attachment; filename=CertificateHealthAndSafety.pdf")
        'Response.BinaryWrite(bytes)  ' Binary data - see myData -  
        'Response.End()

        ' ----------------------------------------------------- Download like Excel

        ' OR

        '' Download   - Excel
        'Dim bytes As Byte() = rv.LocalReport.Render("Excel", Nothing, mimeType, encoding, filenameExtension, streamIds, warnings)
        'Response.Clear()
        'Response.ContentType = "application/excel"
        'Response.AppendHeader("Content-Disposition", "attachment; filename=ExcelHealthAndSafety.xls")
        'Response.BinaryWrite(bytes)  ' Binary data - see myData -  
        'Response.End())


        'https://forums.asp.net/t/1263188.aspx

        'Send email with attached file ...
        'Open in current  window ...
        If Context.Response.IsClientConnected Then
            Context.Response.OutputStream.Write(bytes, 0, bytes.Length)
            Context.Response.Flush()
        End If

        Try
            Dim strTo As String = CStr(Session("Email"))
            ' Add Your Email -FROM   :  HowToSSRSpdfSendEmail 
            SendEmail("test@test.com", strTo, "HowToSSRSpdfSendEmail", "Message - HowToSSRSpdfSendEmail", "", bytes, "HowToSSRSpdfSendEmail.pdf")
        Catch ex As Exception
        End Try

    End Sub

    Public ReadOnly Property MailFromAddress As Net.Mail.MailAddress
        Get
            Return New Net.Mail.MailAddress("test@test.com", "Test : How To PDF from SSRS.")
        End Get
    End Property


    Public Sub SendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMessage As String, ByVal strCC As String, Optional ByRef bytes() As Byte = Nothing, Optional ByVal fileName As String = "")

        If strTo.Trim <> "" Then
            Try
                Dim MailMsg As New MailMessage()
                strTo = strTo.Trim.TrimEnd(CChar(","))
                'strTo = strTo.Trim(CChar(","))
                MailMsg.To.Add(strTo)

                If strCC.Trim <> "" Then
                    MailMsg.CC.Add(strCC)
                End If

                MailMsg.From = MailFromAddress
                MailMsg.BodyEncoding = Encoding.Default
                MailMsg.Subject = strSubject.Trim()
                MailMsg.Body = strMessage.Trim() & vbCrLf
                MailMsg.Priority = MailPriority.Normal
                MailMsg.IsBodyHtml = True

                If Not bytes Is Nothing Then
                    If fileName = "" Then fileName = "AttachmentAddCorrectExtention"
                    Dim att As Attachment = New Attachment(New MemoryStream(bytes), fileName)
                    MailMsg.Attachments.Add(att)
                End If

                ' Add Your SmtpClient ...
                Dim mailClient As New SmtpClient("yyyyy.XXXXX.zzz")  ' <--- ADD YOUR  Your SmtpClient

                mailClient.UseDefaultCredentials = True
                mailClient.Send(MailMsg)
                MailMsg.Dispose()
            Catch innerException As Exception
                Allert(innerException.Message)
            End Try
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