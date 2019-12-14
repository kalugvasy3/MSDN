Option Explicit On 

Imports System.Data.OracleClient 'used for connecting to and using the oracle database
Imports System.Data.SqlClient 'Used for connecting to and using the SQL Server database.

Imports System.Web


Public Class DataAccess

#Region "Functions"

    Private Function getOracleConnection(ByVal strConnectId As String) As OracleConnection
        'set connection string
        Dim strConnectionString As String
        strConnectionString = DetermineConString(strConnectId)

        'create new Oracle Connection
        Dim anOracleConnection As New OracleConnection

        'set connection string property
        anOracleConnection.ConnectionString() = strConnectionString

        'set dbConnection to the oracle connection
        Return anOracleConnection
    End Function



    Private Function getSQLConnection(ByVal strConnectId As String) As SqlConnection
        'set connection string
        Dim strConnectionString As String
        strConnectionString = DetermineConString(strConnectId)

        'create new SQL Server Connection
        Dim anSQLConnection As New SqlConnection

        'set connection string property
        anSQLConnection.ConnectionString() = strConnectionString

        'set dbConnection to the SQL Server connection
        Return anSQLConnection
    End Function

    Private Function DetermineConString(ByVal strConnectId As String) As String
        'set connection string
        Dim strConnectionString As String
        strConnectionString = System.Configuration.ConfigurationSettings.AppSettings(strConnectId)
        Return strConnectionString
    End Function



    Function GetConnStr(ByVal strConnectID As String) As String
        Return DetermineConString(strConnectID)
    End Function


    Function ExecuteSqlScalar(ByVal sql As String, ByVal strConnectId As String, ByVal strIp As String) As Integer
        'variable Declarations
        Dim intResult As Integer
        Dim dbConnectionOra As New OracleConnection

        'establish connection
        dbConnectionOra = getOracleConnection(strConnectId)


        'open the connection
        Try
            dbConnectionOra.Open()
        Catch ex As Exception
            Dim deseDataEx As New DataException("Could not open database connection.  " & sql, sql, ex)
            Throw (deseDataEx)
        End Try

        Dim cmdGetMaxInstance As OracleCommand = New OracleCommand(sql, dbConnectionOra)

        Try
            intResult = cmdGetMaxInstance.ExecuteScalar()
        Catch ex As InvalidCastException
            intResult = 0
        Catch ex As Exception
            dbConnectionOra.Close()

            '----------------------Err----------------------------------------------
            insertSqlException("ORA", strConnectId, ex.Message + "---" + sql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------
            Dim deseDataEx As New DataException("Could not execute query.  " & sql, sql, ex)

            Throw (deseDataEx)
        End Try

        'close database
        dbConnectionOra.Close()
        ExecuteSqlScalar = intResult

    End Function


    Function GetDataSet(ByVal strConnectId As String, ByVal sql As String, ByVal tableName As String, ByVal strIp As String) As DataSet
        'Variable Declarations
        Dim dataSet As New DataSet
        Dim dbAdapterOra As New OracleDataAdapter
        Dim dbConnectionOra As New OracleConnection

        dbConnectionOra = getOracleConnection(strConnectId)

        'create data adapter and set select statement
        dbAdapterOra = New OracleDataAdapter
        dbAdapterOra.SelectCommand = New OracleCommand(sql, dbConnectionOra)

        'open the connection
        Try
            dbConnectionOra.Open()
        Catch ex As Exception
            Dim deseDataEx As New DataException("Could not open database connection when accessing " & tableName & ".  " & sql, sql, ex)
            dataSet.Dispose()
            Throw (deseDataEx)
        End Try

        'Fill data set with results of query
        Try
            dbAdapterOra.Fill(dataSet, tableName)
        Catch ex As Exception
            dbConnectionOra.Close()

            '----------------------Err----------------------------------------------
            insertSqlException("ORA", strConnectId, ex.Message + "---" + sql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------
            Dim deseDataEx As New DataException("Could not retrieve dataset for " & tableName & ".  " & sql, sql, ex)

            dataSet.Dispose()
            Throw (deseDataEx)
        End Try

        dbConnectionOra.Close()
        GetDataSet = dataSet
        dataSet.Dispose()

    End Function

    Sub insertSqlException(ByVal strDb As String, ByVal strConnId As String, ByVal strSql As String, ByVal strIp As String)
        Try
            Dim dbConnection As New SqlConnection
            dbConnection = getSQLConnection("DESE_MS") ' MS SQL CONNECTION STRING from ORACLE WEB.CONFIG  !!!!
            dbConnection.Open()
            Dim Sql As String = "INSERT INTO DESE_UPGRADE_DATABASE_SYNC_ERROR_LOG VALUES (GETDATE(),'" & strConnId & "','" & strDb & "','" & strSql.Replace("'", "''") & "','" & strIp & "')"
            Dim cmd As SqlCommand = New SqlCommand(Sql, dbConnection)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
            End Try
            dbConnection.Close()
        Catch
        End Try

    End Sub


    Sub ExecuteTransaction(ByVal strConnectID As String, ByVal aryCommands() As String, ByVal strIp As String)
        Dim dbConnectionOra As New OracleConnection
        Dim dbConnectionMs As New SqlConnection
        Dim blnMS As Boolean = False
        Dim obj As New UpgradeClassLibrary.OraToSql

        Dim i As Integer

        '----------------------Err----------------------------------------------
        Dim strDb As String = ""
        Dim strSql As String = ""
        '-----------------------------------------------------------------------

        'MS SQL -----------------------------------------------------------------------------
        dbConnectionMs = getSQLConnection(strConnectID + "_MS")
        If dbConnectionMs.ConnectionString <> "" And obj.isContinueSql(aryCommands) Then
            blnMS = True
            Try
                dbConnectionMs.Open()
            Catch ex As Exception
                Throw (ex)
            End Try
        End If
        '------------------------------------------------------------------------------------

        'establish/Open connection

        dbConnectionOra = getOracleConnection(strConnectID)
        Try
            dbConnectionOra.Open()
        Catch ex As Exception

            If blnMS Then
                dbConnectionMs.Close()
            End If

            Throw (ex)
        End Try


        'MS SQL ----start transaction------create command-----------------------------------------------------
        Dim myCommandMs As SqlCommand
        Dim myTransMs As SqlTransaction
        If blnMS Then
            myTransMs = dbConnectionMs.BeginTransaction()
            myCommandMs = dbConnectionMs.CreateCommand()
            myCommandMs.Transaction = myTransMs

            myCommandMs.CommandText = "SELECT GETDATE()"  ' START TRANSACTION
            myCommandMs.ExecuteNonQuery()

        Else
            myCommandMs = Nothing
        End If

        'start transaction     create command
        Dim myTransOra As OracleTransaction = dbConnectionOra.BeginTransaction()
        Dim myCommandOra As OracleCommand = dbConnectionOra.CreateCommand()
        myCommandOra.Transaction = myTransOra

        myCommandOra.CommandText = "SELECT SYSDATE FROM DUAL"  ' START TRANSACTION
        myCommandOra.ExecuteNonQuery()


        Try

            'Loop MS SQL through the array and execute each command
            If blnMS Then

                '-----------------------------------------------------------------------
                'Insert Into Accounting Transaction Line---MsSql------------------------

                'Convert  Old Code to New   -> "Insert Into Accounting Transaction Line"
                Dim objConvert As New UpgradeClassLibrary.OnFly
                Dim aryCommandsMs() As String

                aryCommandsMs = objConvert.aryCommandForMsSql(aryCommands) 'keep old array and create new array for MsSql ' ByRef blnConverted !!!

                'Insert Into Accounting Transaction Line---MsSql------------------------
                '-----------------------------------------------------------------------

                '----------------------Err----------------------------------------------
                strDb = "MS"
                '-----------------------------------------------------------------------

                Dim strIdentity As String = "-1"

                For i = aryCommandsMs.GetLowerBound(0) To aryCommandsMs.GetUpperBound(0)
                    If Trim(aryCommandsMs(i)) = "" Or Trim(aryCommandsMs(i)) = vbNewLine Or Trim(aryCommandsMs(i)) = vbCr Then
                        Continue For
                    End If  'vkalugin
                    myCommandMs.CommandText = obj.ConvertToSQL(aryCommandsMs(i))
                    '----------------------Err----------------------------------------------
                    strSql = myCommandMs.CommandText
                    '-----------------------------------------------------------------------

                    myCommandMs.ExecuteNonQuery()

                    'Insert Into Accounting Transaction Line---For Oracle-----------------------

                    ' If "Accounting Transaction Line" return the last identity values
                    ' The SCOPE_IDENTITY() function will return the null value if the function is invoked before any INSERT statements into an identity column occur in the scope.
                    myCommandMs.CommandText = "SELECT isnull(SCOPE_IDENTITY(),'-1') "
                    Dim strTmp As String = myCommandMs.ExecuteScalar().ToString
                    If strTmp <> "-1" Then
                        strIdentity = strTmp
                    End If


                Next

                '-----------------------------------------------------------------------


                If strIdentity <> "-1" Then
                    'Build code for Oracle  - "Insert Into Accounting Transaction Line"
                    aryCommands = objConvert.aryCommandForOracle(aryCommands, strIdentity)
                End If

                'Insert Into Accounting Transaction Line---For Oracle-----------------------
                '-----------------------------------------------------------------------

            End If

            '----------------------Err----------------------------------------------
            strDb = "ORA"
            '-----------------------------------------------------------------------

            'Loop ORA through the array and execute each command
            For i = aryCommands.GetLowerBound(0) To aryCommands.GetUpperBound(0)
                If Trim(aryCommands(i)) = "" Or Trim(aryCommands(i)) = vbNewLine Or Trim(aryCommands(i)) = vbCr Then
                    Continue For
                End If  'vkalugin
                myCommandOra.CommandText = aryCommands(i)

                '----------------------Err----------------------------------------------
                strSql = myCommandOra.CommandText
                '-----------------------------------------------------------------------

                myCommandOra.ExecuteNonQuery()
            Next
            'Successful...commit the transactions.
            Try
                myTransOra.Commit()
                If blnMS Then
                    myTransMs.Commit()
                End If
            Catch ex As Exception
                Throw (ex)
            End Try 'vkalugin / catch ex. if transaction was closed from sql codes... COMMIT, ROLLBACK.

            dbConnectionOra.Close()
            dbConnectionMs.Close()

        Catch ex As Exception
            'Not successful...rollback the data.
            If blnMS Then
                myTransMs.Rollback()
                dbConnectionMs.Close()
            End If

            myTransOra.Rollback()
            dbConnectionOra.Close()

            '----------------------Err----------------------------------------------
            insertSqlException(strDb, strConnectID, ex.Message + "---" + strSql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------

            Throw (ex)
        End Try

        dbConnectionOra.Close()
        dbConnectionMs.Close()

    End Sub

#End Region
End Class
