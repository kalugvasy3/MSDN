Public Class AddRecord
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub rbSeriousInjury_CheckedChanged(sender As Object, e As EventArgs) Handles rbSeriousInjury.CheckedChanged
        Injury.Text = "Serious"
    End Sub

    Protected Sub rbMinorInjury_CheckedChanged(sender As Object, e As EventArgs) Handles rbMinorInjury.CheckedChanged
        Injury.Text = "Minor"
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click

        Dim strError As String = ""
        Dim strXML As String = ReadValidateControls.GetXMLStringFromAllTextBoxes(pnlMain, strError)

        If strError.Trim <> "" Then
            Allert("Please fill out ""yellow"" fields (use N/A if needed)" + "\n\n" + strError)
            Exit Sub
        End If

        Dim strDateTime As String = Format(Now, "MM-dd-yyyy HH:MM:ss.ffffff")
        InsertNewRecord(strXML, strDateTime)

        pnlMain.Enabled = False
        Allert("Your record was successfully sent. \n\nClose browser!")
    End Sub

    'Your code for Insert XML column to DataBase
    'Example below for DB2 - InsertReport -name of your procedure.
    Sub InsertNewRecord(ByVal strXML As String, ByVal strDateTime As String)

        '    Dim conn As DB2Connection = Nothing
        '    Dim cmd As DB2Command = Nothing
        '    Dim ReportIDPK As Integer = 0

        '    conn = myConn
        '    Try
        '        cmd = New DB2Command(InsertReport, conn)
        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.Parameters.Add("@lclDateTime", DB2Type.Char, 26).Value = strDateTime
        '        cmd.Parameters.Add("@lclRecordData", DB2Type.Xml).Value = strXML

        '        If Not conn.IsOpen Then
        '            conn.Open()
        '        End If
        '        cmd.ExecuteNonQuery()

        '    Catch exc As Exception
        '        Dim msg As String = "Insert Report Error SeriousMinorInjury.  "
        '        msg += exc.Message
        '        Throw New Exception(msg)
        '        Exit Sub
        '    End Try

        '    If conn.IsOpen Then
        '        conn.Close()
        '    End If

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