Imports System.Web.Services
Imports System.Web



<System.Web.Services.WebService(Namespace:="http://tempuri.org/DataAccessService/DataAccessWS")> _
Public Class DataAccessWS
    Inherits System.Web.Services.WebService

#Region " Web Services Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Web Services Designer.
        InitializeComponent()

        'Add your own initialization code after the InitializeComponent() call

    End Sub

    'Required by the Web Services Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Web Services Designer
    'It can be modified using the Web Services Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: This procedure is required by the Web Services Designer
        'Do not modify it using the code editor.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

    '''<summary>
    '''Calls and returns data from the DataAccess class based on the sql passed.
    '''It connects to the database using a connection id. 
    '''</summary>
    '''<returns>
    '''Returns a DataSet
    '''</returns>
    '''<remarks>
    '''<para>Created By: Liza Merriman</para>
    '''<para>Last Modified: 1/27/2004</para>
    '''</remarks>
    ''' 


    <WebMethod()> Public Function Find(ByVal strConnectID As String, ByVal strTableName As String, ByVal strSQL As String) As DataSet
        Dim objFind As New SQLMethods
        Dim ds As New DataSet
        ds = objFind.Find(strConnectID, strTableName, strSQL, Context.Request.UserHostAddress.Trim)
        Try
            Return ds
        Finally
            ds.Dispose()
        End Try
    End Function



    '''<summary>
    '''Executes a an sql query that returns an integer as a result.
    '''<para>An example would be using the MAX function to get the last instance id.</para>
    '''</summary>
    '''<returns>
    '''Returns an Integer.  If no result can be found, a 0 is returned.
    '''</returns>
    '''<remarks>
    '''<para>Created By: Brian Stillfield</para>
    '''<para>Last Modified: 3/16/2005</para>
    '''</remarks>
    <WebMethod()> Public Function FindScalar(ByVal strConnectID As String, ByVal strSQL As String) As Integer
        Dim objFindScalar As New SQLMethods
        FindScalar = objFindScalar.FindScalar(strConnectID, strSQL, Context.Request.UserHostAddress.Trim)
    End Function

    '''<summary>
    '''Updates data using the DataAccess class based on the sql passed.
    '''It connects to the database using a connection id. 
    '''</summary>
    '''<remarks>
    '''<para>Created By: Liza Merriman</para>
    '''<para>Last Modified: 1/27/2004</para>
    '''</remarks>
    <WebMethod()> Public Sub Update(ByVal strConnectID As String, ByVal strSQL As String)
        Dim objUpdate As New SQLMethods
        objUpdate.Update(strConnectID, strSQL, Context.Request.UserHostAddress.Trim)
    End Sub

    '''<summary>
    '''Deletes data using the DataAccess class based on the sql passed.
    '''It connects to the database using a connection id. 
    '''</summary>
    '''<remarks>
    '''<para>Created By: Liza Merriman</para>
    '''<para>Last Modified: 1/27/2004</para>
    '''</remarks>
    <WebMethod()> Public Sub Delete(ByVal strConnectID As String, ByVal strSQL As String)
        Dim objDel As New SQLMethods
        objDel.Delete(strConnectID, strSQL, Context.Request.UserHostAddress.Trim)
    End Sub

    '''<summary>
    '''Creates data using the DataAccess class based on the sql passed.
    '''It connects to the database using a connection id. 
    '''</summary>
    '''<remarks>
    '''<para>Created By: Liza Merriman</para>
    '''<para>Last Modified: 1/27/2004</para>
    '''</remarks>
    <WebMethod()> Public Sub Create(ByVal strConnectID As String, ByVal strSQL As String)
        Dim objCreate As New SQLMethods
        objCreate.Create(strConnectID, strSQL, Context.Request.UserHostAddress.Trim)
    End Sub
    '''<summary>
    '''Processes multiple transactions based on the array passed.
    '''It connects to the database using a connection id. 
    '''</summary>
    '''<remarks>
    '''<para>Created By: Liza Merriman</para>
    '''<para>Last Modified: 8/10/2005</para>
    '''</remarks>
    <WebMethod()> Public Sub ProcessTransaction(ByVal strConnectID As String, ByVal aryCommands() As String)
        Dim objProcessTrans As New SQLMethods
        objProcessTrans.ProcessTransaction(strConnectID, aryCommands, Context.Request.UserHostAddress.Trim)
    End Sub


    ''' <summary>
    ''' Returns the Connection string used to access the database.
    ''' based on the ConnectID passed in.
    ''' </summary>
    ''' <para>Created By: James Reiha</para>
    ''' <para>Last Modified: 2/18/2010</para>
    <WebMethod()> Public Function GetConnString(ByVal strConnectID As String) As String
        Dim objGetConnString As New SQLMethods
        Return objGetConnString.GetConnString(strConnectID)
    End Function


End Class
