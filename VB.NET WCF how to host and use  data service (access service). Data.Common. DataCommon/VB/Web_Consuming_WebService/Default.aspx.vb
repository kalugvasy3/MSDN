Imports System.ServiceModel

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSchema_Click(sender As Object, e As EventArgs) Handles btnSchema.Click

        'We can use only below if you create actual Service Reference from actual service
        'Dim srv As New serviceDMH.SqlServiceContractClient()

        ' Can skip if use Default Setting
        Dim binding As BasicHttpBinding = New BasicHttpBinding
        binding.MaxBufferPoolSize = 2147483647
        binding.MaxReceivedMessageSize = 2147483647
        binding.MaxBufferSize = 2147483647

        ' Actual Service Address ... Replace SSIJCEDU8046967 with your computer name ...  
        Dim endpointAddress As EndpointAddress = New EndpointAddress("http://SSIJCEDU8046967:8080/WindowsServiceSQL")


        'Below rewrite wsdl setting ...
        Dim srv As New serviceSQL.SqlServiceContractClient(binding, endpointAddress)
        ' End Section - skip - uncomment above ...

        Dim sqlIn As New serviceSQL.SqlDataIn ' See WCFhostVBnet
        Dim sqlOut As New serviceSQL.SqlDataOut

        sqlIn.SqlParam = Nothing  ' if you do not use it just skip or Nothing
        sqlIn.SqlType = "Text" ' It can be "" or "Text" | if you use rocedure "Proc"
        sqlIn.SqlText = txtSQL.Text

        ' MS SQL
        'sqlIn.SqlText = " SELECT  * FROM INFORMATION_SCHEMA.COLUMNS"   ' Sql Code ... if you run PROC you can use CALL procname (param1, ... , paramN) - Do not use in that case sqlParam

        ' DB2 below
        'sqlIn.SqlText = " SELECT  *  FROM SYSCAT.TABLES WHERE OWNERTYPE='U'"   '


        Try
            sqlOut = srv.GetDataSet(sqlIn)
        Catch ex As Exception
            sqlOut.strError = ex.Message
        End Try

        If sqlOut.strError = "" Then
            Try
                grdSchema.DataSource = sqlOut.DataSetOut
                grdSchema.DataBind()
                txtError.Text = "OK"
            Catch ex As Exception
                txtError.Text = ex.Message
            End Try
        Else
            grdSchema.DataSource = Nothing
            grdSchema.DataBind()
            txtError.Text = sqlOut.strError
        End If

    End Sub

End Class