

Public Class SQLMethods
    Inherits DataAccess



    Public Function Find(ByVal strConnectID As String, ByVal strTableName As String, ByVal strSQL As String, ByVal strIp As String) As DataSet

        Find = GetDataSet(strConnectID, strSQL, strTableName, strIp)


    End Function


    Public Function FindScalar(ByVal strConnectID As String, ByVal strSQL As String, ByVal strIp As String) As Integer
        FindScalar = ExecuteSqlScalar(strSQL, strConnectID, strIp)
    End Function


    Protected Function semicolon(ByVal strSQL As String) As String

        Dim chs() As Char = strSQL.ToCharArray

        Dim blnReplace As Boolean = True
        For i As Integer = 0 To strSQL.Length - 1

            If chs(i) = "'" And blnReplace = True Then
                blnReplace = False
            ElseIf chs(i) = "'" And blnReplace = False Then
                blnReplace = True
            End If

            If chs(i) = ";" And blnReplace Then
                chs(i) = vbNullChar
            End If
        Next
        Return chs

    End Function

    Public Sub Update(ByVal strConnectID As String, ByVal strSQL As String, ByVal strIp As String)

        'ExecuteSqlNonQuery(strSQL, strConnectID)
        Dim aryCommands() As String
        strSQL = semicolon(strSQL)
        aryCommands = strSQL.Split(vbNullChar)
        ExecuteTransaction(strConnectID, aryCommands, strIp)
    End Sub

    '''<summary>
    '''  Deletes data based on the sql that is passed. 
    '''</summary>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 8/14/2007 </para>
    '''</remarks>

    Public Sub Delete(ByVal strConnectID As String, ByVal strSQL As String, ByVal strIp As String)

        'ExecuteSqlNonQuery(strSQL, strConnectID)
        Dim aryCommands() As String
        strSQL = semicolon(strSQL)
        aryCommands = strSQL.Split(vbNullChar)
        ExecuteTransaction(strConnectID, aryCommands, strIp)

    End Sub

    '''<summary>
    '''  Creates records based on the sql that is passed. 
    '''</summary>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 8/14/2007 </para>
    '''</remarks>

    Public Sub Create(ByVal strConnectID As String, ByVal strSQL As String, ByVal strIp As String)

        'ExecuteSqlNonQuery(strSQL, strConnectID)
        Dim aryCommands() As String
        strSQL = semicolon(strSQL)
        aryCommands = strSQL.Split(vbNullChar)
        ExecuteTransaction(strConnectID, aryCommands, strIp)

    End Sub

    '''<summary>
    '''  Processes transactions based on the array passed in. 
    '''</summary>
    '''<remarks>
    '''  <para> Created By: Shirley Fowler </para>
    '''  <para> Last Modified: 8/14/2007 </para>
    '''</remarks>

    Public Sub ProcessTransaction(ByVal strConnectID As String, ByVal aryCommands() As String, ByVal strIp As String)

        ExecuteTransaction(strConnectID, aryCommands, strIp)

    End Sub



End Class
