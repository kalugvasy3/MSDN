Public Class MainControl
    Inherits System.Web.UI.UserControl

    Private cf As CommonClassLibrary.CommonFunction
    ' XML include all VALUEs for ALL Web Controls
    Private _XML As String


    ' Each MainControl (each UserControl) should has owner XML
    Public Property XML As String
        Get
            Return _XML
        End Get
        Set(ByVal value As String)
            If Session("CF") Is Nothing Then
                cf = New CommonClassLibrary.CommonFunction
            Else
                cf = CType(Session("CF"), CommonClassLibrary.CommonFunction)
            End If

            _XML = value


            cf.loadAll(Me.Controls, _XML)
            Session("CF") = cf

        End Set
    End Property


    ' Init All Grids itself - events, No Data (load data must be in Page_Load - Not PostBack)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("CF") Is Nothing Then
            cf = New CommonClassLibrary.CommonFunction
        Else
            cf = CType(Session("CF"), CommonClassLibrary.CommonFunction)
        End If

        ' Add here INIT for all Grid (DO NOT ASSIGN / SET DATA here)

        ' GridView MUST be separate from any panel (do not include/combine GridView with Panel - Panel means separarte table in XML)

        For Each cnt As Control In Me.Controls
            If TypeOf (cnt) Is GridView Then
                Dim grd As GridView = CType(cnt, GridView)
                cf.initAllGridViewWithHandler(grd)   ' Init Events ONLY
            End If
        Next
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Load Data
        If Not Page.IsPostBack Then

            ' LOAD DATA SHOULD BE HERE
            initAllGridViewWithData() ' Load Data
        End If

    End Sub

    Private Sub initAllGridViewWithData()

        Dim dt As New DataTable

        If Session("CF") Is Nothing Then
            cf = New CommonClassLibrary.CommonFunction
            Session("CF") = cf
        End If
        cf = CType(Session("CF"), CommonClassLibrary.CommonFunction)

        For Each cnt As Control In Me.Controls
            If TypeOf (cnt) Is GridView Then
                Dim grd As GridView = CType(cnt, GridView)
                Try
                    dt = cf.DS.Tables(grd.ID)
                Catch ex As Exception
                    dt = Nothing
                End Try

                cf.initGridView(grd, dt)   ' Init Data ONLY or add empty Row
            End If
        Next

    End Sub


    Protected Sub btnResetDemographic_Click(sender As Object, e As EventArgs) Handles btnResetDemographic.Click
        cf.resetControls(pnlDemographic)
    End Sub

    Protected Sub btnResetSkills_Click(sender As Object, e As EventArgs) Handles btnResetSkills.Click
        cf.resetControls(pnlSkills)
    End Sub

    ''' <summary>
    ''' Below Insert XML 
    ''' </summary>
    ''' <returns></returns>
    Public Function insertNewRow() As String
        Dim ds As New DataSet
        Dim strError As String = ""
        Dim strXML As String = cf.convertToXml(Me.Controls, strError) ' Send PlaceHolder to convertToXml 
        Session("CF") = cf

        If strError <> "" Then Return strError

        ' SQLite syntax - you need to replace to your DB syntax ...
        Dim strSqLInsert As String = "  
             INSERT INTO ""FastPrototypeWebSite""
             (""PK_INDEX"", ""XML"",""DATE"") 
             SELECT ifnull(MAX ( ""PK_INDEX""),  0) + 1 AS ""PK_INDEX""  ,  
             ' " + strXML.Replace("'", "''") + "' AS ""XML"" , '" + Format(Now, "MM/dd/yyyy") + "' AS ""DATE"" FROM ""FastPrototypeWebSite"" "

        Dim dc As New CommonClassLibrary.DataCommon ' it create connection to DataBase -- see web.config
        Dim com = dc.commandDB
        com.CommandText = strSqLInsert

        dc.ExecuteSqlNonQuery(com)
        Return ""
    End Function


    ''' <summary>
    ''' Update XML column
    ''' </summary>
    ''' <param name="pk_index"></param>
    ''' <returns></returns>
    Public Function updateRow(ByRef pk_index As Integer) As String
        Dim strError As String = ""
        Dim ds As DataSet = New DataSet
        cf = CType(Session("CF"), CommonClassLibrary.CommonFunction)
        Dim strXML As String = cf.convertToXml(Me.Controls, strError) ' Send PlaceHolder to convertToXml 
        Session("CF") = cf

        If strError <> "" Then Return strError

        ' SQLite syntax - you need to replace to your DB syntax ...
        Dim strSqLInsert As String = "  
             UPDATE ""FastPrototypeWebSite""
             SET ""XML"" = '" + strXML + "' 
             WHERE ""PK_INDEX"" = " + pk_index.ToString

        Dim dc As New CommonClassLibrary.DataCommon ' it create connection to DataBase -- see web.config
        Dim com = dc.commandDB
        com.CommandText = strSqLInsert
        dc.ExecuteSqlNonQuery(com)
        Return ""

    End Function


End Class