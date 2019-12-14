'''<summary>
'''  DESE Specific Exception Class to be used for handling Errors that occur when accessing 
'''  the database.  This class will allow the SQL statement that caused the error to be
'''  accessed and displayed from the calling application and allow data access errors to be
'''  handled in specific manner.
'''</summary>

Public Class DataException
    Inherits System.ApplicationException

    Private m_strSQL

#Region "Constructors"

    '''<summary>
    '''  Constructer used to create an instance of a DataException.
    '''  All parameters must be passed.
    '''</summary>
    '''<remarks>
    '''  <para>Created By: Shirley Fowler - August 14, 2007 </para>
    '''</remarks>

    Sub New(ByVal strErrorMsg As String, ByVal sql As String, _
            ByVal innerException As Exception)
        MyBase.New(strErrorMsg, innerException)
        m_strSQL = sql
    End Sub
#End Region

#Region "Properties"
    '''<summary>
    '''  Gets the sql statement that was attempted when the exception occurred.
    '''</summary>

    ReadOnly Property sql()
        Get
            sql = m_strSQL
        End Get
    End Property
#End Region
End Class
