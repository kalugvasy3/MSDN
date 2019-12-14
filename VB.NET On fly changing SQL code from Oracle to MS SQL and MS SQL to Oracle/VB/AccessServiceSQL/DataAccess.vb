Option Explicit On 

Imports System.Data.SqlClient 'Used for connecting to and using the SQL Server database.
Imports System.Data.OracleClient 'used for connecting to and using the oracle database

Imports System.Web

'Author: Shirley Fowler for Bobbie Sue's DESE e-Mail Bag Project.
'Created Date: 8/14/07

'''<summary>
'''  DataAccess is the base class for any classes that require access to a database table.  
'''  It provides functionality for selecting, updating, inserting, and deleting records
'''  from the database.
'''</summary>

Public Class DataAccess

#Region "Functions"
    '''<summary>
    '''  Establishes a SQL Server connection.
    '''</summary>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 7/14/2007 </para>
    '''</remarks>

    Private Function getSQLConnection(ByVal strConnectId As String) As SqlConnection
        'set connection string
        Dim strConnectionString As String
        strConnectionString = DetermineConString(strConnectId)

        'create new SQL Server Connection
        Dim anSQLConnection As New SqlConnection

        'set connection string property
        anSQLConnection.ConnectionString() = strConnectionString

        'set dbConnectionMs to the SQL Server connection
        Return anSQLConnection
    End Function


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

    '''<summary>
    '''  Determines which connection string.
    '''</summary>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 8/14/2007 - Shirley Fowler </para>
    '''  <para> Put connection strings in web.config file</para>
    '''</remarks>

    Private Function DetermineConString(ByVal strConnectId As String) As String
        'set connection string
        Dim strConnectionString As String
        strConnectionString = System.Configuration.ConfigurationSettings.AppSettings(strConnectId)
        Return strConnectionString
    End Function


    ''' <summary>
    ''' Returns the connection string based on the ConnectID passed in
    ''' </summary>
    ''' <para>Created By: James Reiha</para>
    ''' <para>Last Modified: 2/18/2010</para>
    'Function GetConnStr(ByVal strConnectID As String) As String
    '    Return DetermineConString(strConnectID)
    'End Function


    '''<summary>
    '''  Executes a an sql query that returns an integer as a result.
    '''  An example would be using the MAX function to get the last instance id.
    '''</summary>
    '''<returns>
    '''  Returns an Integer.  If no result can be found, a 0 is returned.
    '''</returns>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 8/14/2007</para>
    '''</remarks>

    Function ExecuteSqlScalar(ByVal sql As String, ByVal strConnectId As String, ByVal strIp As String) As Integer
        'variable Declarations
        Dim intResult As Integer
        Dim dbConnectionMs As New SqlConnection

        'establish connection
        dbConnectionMs = getSQLConnection(strConnectId)

        'open the connection
        Try
            dbConnectionMs.Open()
        Catch ex As Exception
            Dim deseDataEx As New DataException("Could not open database connection.  " & sql, sql, ex)
            Throw (deseDataEx)
        End Try

        Dim cmdGetMaxInstance As SqlCommand = New SqlCommand(sql, dbConnectionMs)

        Try
            intResult = cmdGetMaxInstance.ExecuteScalar()
        Catch ex As InvalidCastException
            intResult = 0
        Catch ex As Exception
            dbConnectionMs.Close()

            '----------------------Err----------------------------------------------
            insertSqlException("MS", strConnectId, ex.Message + "---" + sql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------


            Dim deseDataEx As New DataException("Could not execute query.  " & sql, sql, ex)
            Throw (deseDataEx)
        End Try

        'close database
        dbConnectionMs.Close()
        ExecuteSqlScalar = intResult

    End Function


    '''<summary>
    '''  Retrieves a DataSet based on the SELECT sql statement and returns a dataset
    '''  with the query results.
    '''</summary>
    '''<returns>
    '''  Returns a DataSet
    '''</returns>
    '''<remarks>
    '''  <para>Created By: Shirley Fowler</para>
    '''  <para>Last Modified: 8/14/2007</para>
    '''</remarks>

    Function GetDataSet(ByVal strConnectId As String, ByVal sql As String, ByVal tableName As String, ByVal strIp As String) As DataSet
        'Variable Declarations
        Dim dataSet As New DataSet
        Dim dbAdapter As New SqlDataAdapter
        Dim dbConnectionMs As New SqlConnection

        dbConnectionMs = getSQLConnection(strConnectId)

        'create data adapter and set select statement
        dbAdapter = New SqlDataAdapter
        dbAdapter.SelectCommand = New SqlCommand(sql, dbConnectionMs)

        'open the connection
        Try
            dbConnectionMs.Open()
        Catch ex As Exception
            Dim deseDataEx As New DataException("Could not open database connection when accessing " & tableName & ".  " & sql, sql, ex)
            dataSet.Dispose()
            Throw (deseDataEx)
        End Try

        'Fill data set with results of query
        Try
            dbAdapter.Fill(dataSet, tableName)
        Catch ex As Exception
            dbConnectionMs.Close()

            '----------------------Err----------------------------------------------
            insertSqlException("MS", strConnectId, ex.Message + "---" + sql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------


            Dim deseDataEx As New DataException("Could not retrieve dataset for " & tableName & ".  " & sql, sql, ex)
            dataSet.Dispose()
            Throw (deseDataEx)
        End Try

        dbConnectionMs.Close()
        GetDataSet = dataSet
        dataSet.Dispose()

    End Function

    '''<summary>
    '''  Executes transactions.
    '''  Used for multiple insert, update, and delete statements.
    '''</summary>
    '''<remarks>
    '''  <para>Created By: Shirley Fowler</para>
    '''  <para>Last Modified: 8/14/2007</para>
    '''</remarks>

    Sub insertSqlException(ByVal strDb As String, ByVal strConnId As String, ByVal strSql As String, ByVal strIp As String)
        Dim dbConnection As New SqlConnection
        dbConnection = getSQLConnection("DESE")
        dbConnection.Open()
        Dim Sql As String = "INSERT INTO DESE_UPGRADE_DATABASE_SYNC_ERROR_LOG VALUES (GETDATE(),'" & strConnId & "','" & strDb & "','" & strSql.Replace("'", "''") & "','" & strIp & "')"
        Dim cmd As SqlCommand = New SqlCommand(Sql, dbConnection)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        End Try
        dbConnection.Close()
    End Sub


    Sub ExecuteTransaction(ByVal strConnectID As String, ByVal aryCommands() As String, ByVal strIp As String)
        Dim dbConnectionMs As New SqlConnection
        Dim dbConnectionOra As New OracleConnection
        Dim blnOra As Boolean = False

        Dim i As Integer
        Dim obj As New UpgradeClassLibrary.SqlToOra

        '----------------------Err----------------------------------------------
        Dim strDb As String = ""
        Dim strSql As String = ""
        '-----------------------------------------------------------------------

        'MS ORA -----------------------------------------------------------------------------
        dbConnectionOra = getOracleConnection(strConnectID + "_ORA")
        If dbConnectionOra.ConnectionString <> "" And obj.isContinueOra(aryCommands) Then
            blnOra = True
            Try
                dbConnectionOra.Open()
            Catch ex As Exception
                Throw (ex)
            End Try
        End If
        '------------------------------------------------------------------------------------

        'establish connection   open the connection
        dbConnectionMs = getSQLConnection(strConnectID)

        Try
            dbConnectionMs.Open()
        Catch ex As Exception

            If blnOra Then
                dbConnectionOra.Close()
            End If
            Throw (ex)
        End Try


        'MS ORA ----start transaction------create command-----------------------------------------------------
        Dim myCommandOra As OracleCommand
        Dim myTransOra As OracleTransaction
        If blnOra Then
            myTransOra = dbConnectionOra.BeginTransaction()
            myCommandOra = dbConnectionOra.CreateCommand()
            myCommandOra.Transaction = myTransOra

            myCommandOra.CommandText = "SELECT SYSDATE FROM DUAL"  ' START TRANSACTION 
            myCommandOra.ExecuteNonQuery()
        Else
            myCommandOra = Nothing
        End If

        'start transaction
        Dim myTransMs As SqlTransaction = dbConnectionMs.BeginTransaction()

        'create command
        Dim myCommandMs As SqlCommand = dbConnectionMs.CreateCommand()
        myCommandMs.Transaction = myTransMs

        myCommandMs.CommandText = "SELECT GETDATE()"  ' START TRANSACTION
        myCommandMs.ExecuteNonQuery()

        Try
            '-----------------------------------------------------------------------
            'Insert Into Accounting Transaction Line---MsSql------------------------

            'Convert  Old Code to New   -> "Insert Into Accounting Transaction Line"
            Dim objConvert As New UpgradeClassLibrary.OnFly
            Dim aryCommandsMs() As String

            If blnOra Then
                aryCommandsMs = objConvert.aryCommandForMsSql(aryCommands) 'keep old array and create new array for MsSql ' ByRef blnConverted !!!
            Else
                aryCommandsMs = aryCommands 'if do not need to update/insert ORA - keep command AS IS (do not low/upper case)
            End If


            'Insert Into Accounting Transaction Line---MsSql------------------------
            '-----------------------------------------------------------------------

            '----------------------Err----------------------------------------------
            strDb = "MS"
            '------------------ aryCommandsMs --------------------------------------

            Dim strIdentity As String = "-1"

            'Loop through the array and execute each command
            For i = aryCommandsMs.GetLowerBound(0) To aryCommandsMs.GetUpperBound(0)
                If Trim(aryCommandsMs(i)) = "" Or Trim(aryCommandsMs(i)) = vbNewLine Or Trim(aryCommandsMs(i)) = vbCr Then
                    Continue For
                End If  'vkalugin
                myCommandMs.CommandText = aryCommandsMs(i)

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

            'Insert Into Accounting Transaction Line---Oracle-----------------------
            '-----------------------------------------------------------------------

            If blnOra Then
                '----------------------Err----------------------------------------------
                strDb = "ORA"
                '-----------------------------------------------------------------------

                For i = aryCommands.GetLowerBound(0) To aryCommands.GetUpperBound(0)
                    If Trim(aryCommands(i)) = "" Or Trim(aryCommands(i)) = vbNewLine Or Trim(aryCommands(i)) = vbCr Then
                        Continue For
                    End If  'vkalugin
                    myCommandOra.CommandText = obj.ConvertToORA(aryCommands(i))

                    '----------------------Err----------------------------------------------
                    strSql = myCommandOra.CommandText
                    '-----------------------------------------------------------------------

                    myCommandOra.ExecuteNonQuery()
                Next
            End If


            'Successful...commit the transactions.
            Try
                myTransMs.Commit()
                If blnOra Then
                    myTransOra.Commit()
                End If
            Catch ex As Exception
                Throw (ex)
            End Try 'vkalugin / catch ex. if transaction was closed from sql codes... COMMIT, ROLLBACK.


        Catch ex As Exception
            'Not successful...rollback the data.
            If blnOra Then
                myTransOra.Rollback()
                dbConnectionOra.Close()
            End If

            myTransMs.Rollback()
            dbConnectionMs.Close()
            '----------------------Err----------------------------------------------
            insertSqlException(strDb, strConnectID, ex.Message + "---" + strSql, strIp)  ' INSERT ERROR SQL_CODE
            '-----------------------------------------------------------------------

            Throw (ex)
        End Try

        If blnOra Then
            dbConnectionOra.Close()
        End If
        dbConnectionMs.Close()

    End Sub

#End Region
End Class

