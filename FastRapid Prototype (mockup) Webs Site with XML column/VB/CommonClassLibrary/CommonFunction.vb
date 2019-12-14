Imports System.Web
Imports System.Net.Mail

Imports System.Web.UI.WebControls
Imports System.Text
Imports System.IO
Imports System.Web.UI

Public Class CommonFunction
    Inherits System.Web.UI.UserControl

    Private _ds As DataSet

    Public Property DS As DataSet
        Get
            If _ds Is Nothing Then
                _ds = New DataSet
            End If
            Return _ds
        End Get
        Set(ByVal value As DataSet)
            _ds = value
        End Set
    End Property


    Public Sub resetControls(ByRef pnl As Panel)

        For Each control In pnl.Controls   ' control As Object

            If TypeOf (control) Is TextBox Then
                Dim txtBox As TextBox = CType(control, TextBox)
                txtBox.BackColor = Drawing.Color.White
                txtBox.Text = ""
            End If


            If TypeOf (control) Is DropDownList Then
                Dim ddl As DropDownList = CType(control, DropDownList)
                ddl.BackColor = Drawing.Color.White
                ddl.SelectedIndex = -1
            End If

            If TypeOf (control) Is ListBox Then
                Dim list As ListBox = CType(control, ListBox)
                list.BackColor = Drawing.Color.White
                list.SelectedIndex = -1
            End If

            If TypeOf (control) Is GridView Then
                Dim grid As GridView = CType(control, GridView)
                grid.BackColor = Drawing.Color.White
                Dim dt As DataTable = buildGridDataTableFromGrid(grid)
                dt.Rows.Add(dt.NewRow)
                grid.DataSource = dt
                grid.SelectedIndex = -1
                grid.EditIndex = -1
            End If

            If TypeOf (control) Is CheckBox Then
                Dim cb As CheckBox = CType(control, CheckBox)
                cb.BackColor = Drawing.Color.White
                cb.Checked = False
            End If

            ' Add new component here ...

        Next

    End Sub


    Public Sub resetAllControls(ByRef contrCollect As UI.ControlCollection)

        For Each contr In contrCollect  ' contr As Object
            If TypeOf (contr) Is Panel Then
                Dim pnl As Panel = CType(contr, Panel)
                resetControls(pnl)
            End If

            If TypeOf (contr) Is GridView Then
                Dim grd As GridView = CType(contr, GridView)
                initGridView(grd, Nothing)
            End If

        Next

    End Sub

    ''' <summary>
    ''' Load All Controls in Panel
    ''' TextBox, DropDownList, ListBox
    ''' </summary>
    ''' <param name="contrCollect"></param>
    ''' <param name="tbl"></param>
    Public Sub loadPanelControls(ByRef contrCollect As UI.ControlCollection, ByRef tbl As DataTable)

        Dim row As DataRow = tbl.Rows(0)  ' It MUSt include only one ROW

        For Each contr In contrCollect ' contr As Object

            If TypeOf (contr) Is TextBox Then
                Dim txtBox As TextBox = CType(contr, TextBox)
                txtBox.BackColor = Drawing.Color.White
                Try
                    txtBox.Text = row(txtBox.ID)
                Catch ex As Exception
                    txtBox.Text = ""
                End Try
            End If

            If TypeOf (contr) Is DropDownList Then
                Dim ddl As DropDownList = CType(contr, DropDownList)
                ddl.BackColor = Drawing.Color.White
                Try
                    ddl.SelectedValue = row(ddl.ID)
                Catch ex As Exception
                    ddl.SelectedIndex = -1
                End Try
            End If

            If TypeOf (contr) Is ListBox Then
                Dim list As ListBox = CType(contr, ListBox)
                list.ClearSelection()
                list.BackColor = Drawing.Color.White
                list.SelectionMode = ListSelectionMode.Multiple
                Dim strList As String = ""
                Try
                    strList = row(list.ID)
                Catch ex As Exception
                    strList = ""
                End Try

                For Each str As String In strList.Split(";")
                    If str <> "" Then
                        For Each item As ListItem In list.Items
                            If item.Text = str Then
                                item.Selected = True
                            End If
                        Next
                    End If
                Next

            End If


            If TypeOf (contr) Is CheckBox Then
                Dim cb As CheckBox = CType(contr, CheckBox)
                cb.Checked = False
                Try
                    cb.Checked = CBool(row(cb.ID))
                Catch ex As Exception
                End Try
            End If

            ' Add new component here ...

        Next
    End Sub

#Region "XML"

    Private xmlConvertor As New XmlConvertReadWrite

    Public Function loadAll(ByRef contrCollect As UI.ControlCollection, ByRef strXML As String) As DataSet

        'Reset before load DataSet from XML
        resetAllControls(contrCollect)

        If strXML = "" Then
        Else
            'Clear Tables / Keep structure
            For Each t As DataTable In DS.Tables
                t.Clear()
            Next
            DS.Merge(xmlConvertor.GetDataSetFromXml(strXML))
        End If

        ' Each Table represent Panel or Grid
        For Each t As DataTable In DS.Tables
            For Each contr In contrCollect
                If TypeOf (contr) Is Panel Then
                    Dim pnl As Panel = CType(contr, Panel)
                    If t.TableName = pnl.ID Then
                        pnl.Visible = True
                        loadPanelControls(pnl.Controls, t)
                    End If
                End If

                ' load All GridView
                If TypeOf (contr) Is GridView Then
                    Dim grd As GridView = CType(contr, GridView)
                    If t.TableName = grd.ID Then
                        grd.Visible = True
                        grd.DataSource = t
                        grd.DataBind()
                    End If
                End If
            Next
        Next

        Return DS
    End Function

    'it uses in MainControl (for reduce number of references)
    Public Function convertToDataSet(ByRef strXML As String) As DataSet
        Return xmlConvertor.GetDataSetFromXml(strXML)
    End Function


    ''' <summary>
    ''' Convert All Controls to XML
    ''' One Panel - one table with pnl.ID name
    ''' each GridView - separate table (do not wrap GridView with Panel) 
    ''' </summary>
    ''' <param name="contrCollect"></param>
    ''' <param name="strError"></param>
    ''' <returns></returns>
    Public Function convertToXml(ByRef contrCollect As UI.ControlCollection, Optional ByRef strError As String = "") As String

        Dim strXML As String = ""

        If DS Is Nothing Then DS = New DataSet
        DS.DataSetName = "XML"

        Dim originTable As New DataTable

        For Each cnt In contrCollect   ' For Each Controls - wrapped panel(s), gridView(s)

            ' Each Panel is equivalent a Table..
            ' Each GridView has separate Table ...

            Dim strPanelName As String = ""
            ' Section for Panel
            If TypeOf (cnt) Is Panel Then
                Dim pnl As Panel = CType(cnt, Panel)

                originTable = New DataTable(pnl.ID.ToString)
                strPanelName = pnl.ID.ToString

                ' Create DataTble base on simple UserControl(s)
                For Each control In pnl.Controls

                    If TypeOf (control) Is TextBox Then
                        Dim txtBox As TextBox = CType(control, TextBox)
                        Dim columnName As String = txtBox.ID.ToString  'Use word "Null" inside Name - Value Can be Null (uses in validation process)
                        originTable.Columns.Add(columnName, Type.GetType("System.String"))
                    End If

                    If TypeOf (control) Is DropDownList Then
                        Dim ddl As DropDownList = CType(control, DropDownList)
                        Dim columnName As String = ddl.ID.ToString
                        originTable.Columns.Add(columnName, Type.GetType("System.String"))
                    End If

                    ' ListBox can represent and set of checkBoxes (if needed - use template)
                    If TypeOf (control) Is ListBox Then
                        Dim list As ListBox = CType(control, ListBox)
                        Dim columnName As String = list.ID.ToString()
                        originTable.Columns.Add(columnName, Type.GetType("System.String"))
                    End If

                    ' CheckBox can represent and set of checkBoxes (if needed - use template)
                    If TypeOf (control) Is CheckBox Then
                        Dim list As CheckBox = CType(control, CheckBox)
                        Dim columnName As String = list.ID.ToString()
                        originTable.Columns.Add(columnName, Type.GetType("System.String"))
                    End If

                    ' Add new component here ...
                    '  ...

                    ' For GridView see below ... will be separate table inside dataset ...)
                    '  ...
                Next


                'Populate Table by Value(s) - with Validation ... (uses word "Null" in user control Name for identify "Null" value)
                Dim row As DataRow = originTable.NewRow

                For Each control In pnl.Controls
                    If TypeOf (control) Is TextBox Then
                        Dim txtBox As TextBox = CType(control, TextBox)
                        Dim columnName As String = txtBox.ID
                        If columnName.IndexOf("Null") < 0 Then
                            If txtBox.Text.Trim() = "" Then
                                strError += "TextBox " + txtBox.ID + " should not be empty. \n"
                                txtBox.BackColor = Drawing.Color.Yellow
                            Else
                                txtBox.BackColor = Drawing.Color.White
                            End If
                        End If
                        row(columnName) = txtBox.Text
                    End If

                    If TypeOf (control) Is DropDownList Then
                        Dim ddl As DropDownList = CType(control, DropDownList)
                        Dim columnName As String = ddl.ID
                        If columnName.IndexOf("Null") < 0 Then
                            If ddl.SelectedIndex <= 0 Then
                                strError += "DropDownList " + ddl.ID + " please select something. \n"
                                ddl.BackColor = Drawing.Color.Yellow
                            Else
                                ddl.BackColor = Drawing.Color.White
                            End If
                        End If
                        row(columnName) = ddl.SelectedValue
                    End If

                    If TypeOf (control) Is ListBox Then
                        Dim list As ListBox = CType(control, ListBox)
                        Dim columnName As String = list.ID.ToString()

                        Dim strSelected As String = ""
                        Dim selectedIndexes() As Integer = list.GetSelectedIndices()

                        If columnName.IndexOf("Null") < 0 Then   ' if  not null something select.
                            If selectedIndexes.Length = 0 Then
                                strError += "ListBox " + list.ID + " please select something. \n"
                                list.BackColor = Drawing.Color.Yellow
                            Else
                                list.BackColor = Drawing.Color.White
                            End If
                        End If

                        ' Combine ALL selected value to one string with separator ";"
                        For Each intS As Integer In selectedIndexes
                            strSelected += list.Items(intS).Text + ";"
                        Next

                        row(columnName) = strSelected
                    End If


                    If TypeOf (control) Is CheckBox Then
                        Dim cb As CheckBox = CType(control, CheckBox)
                        Dim columnName As String = cb.ID.ToString()

                        Dim bln As Boolean = False
                        If cb.Checked Then
                            bln = True
                        End If

                        row(columnName) = bln.ToString
                    End If

                    ' Add new component here ...

                Next

                originTable.Rows.Add(row)  ' "One Row to Table"

                ' Remove table if exist
                Try
                    DS.Tables.Remove(originTable.TableName)
                Catch ex As Exception
                End Try

                DS.Tables.Add(originTable) '  One Panel one Table (Each panel has owner table inside xml document)
                DS.AcceptChanges()

            End If


            If TypeOf (cnt) Is GridView Then
                ' Dim grd As GridView = CType(cnt, GridView)
                ' initGridView(grd, DS.Tables(grd.ID))
            End If
        Next

        ' Convert DataSet (for particular PK_Index) to XML documents.
        strXML = xmlConvertor.GetXMLString(DS)
        Return strXML

    End Function

    ' Build DataTable structure base on DataGrid  
    Public Function buildGridDataTableFromGrid(ByRef grdView As GridView) As DataTable

        Dim dt As New DataTable
        dt.TableName = grdView.ID.ToString

        For Each dcf As DataControlField In grdView.Columns
            Dim columnName As String = dcf.HeaderText
            If columnName = "" Then Continue For
            Dim col As DataColumn = New DataColumn(columnName)
            dt.Columns.Add(col)
        Next
        Return dt
    End Function

    '' Grid View Section(s)
    '' Init GridView (Always use ByRef)

    Public Sub initGridView(ByRef grdView As GridView, ByRef obj As Object)  'Init Grid View base Column Name
        ' obj - can be Nothing, XML, DataTable

        Dim dt As New DataTable

        ' If String  1st place
        If TypeOf (obj) Is String Then
            Dim str As String = CStr(obj)
            DS = xmlConvertor.GetDataSetFromXml(str)
            If DS.Tables(grdView.ID) Is Nothing Or DS.Tables(grdView.ID).Rows.Count = 0 Then
                dt = buildGridDataTableFromGrid(grdView)
                Try ' Prevent crush application if previous data does not have this table (if dynamically add/delete new elements)
                    DS.Tables.Remove(grdView.ID)
                Catch ex As Exception
                End Try
                DS.Tables.Add(dt)
            End If

            ' If dt  2nd place
        ElseIf TypeOf (obj) Is DataTable Then
            ' DS may not include this table
            dt = CType(obj, DataTable)
            Try ' Prevent crush application if previous data does not have this table (if dynamically add/delete new elements)
                DS.Tables.Remove(grdView.ID)
            Catch ex As Exception
            End Try
            DS.Tables.Add(dt)

            ' if AddNew - just add empty row
        ElseIf obj Is Nothing Then
            dt = buildGridDataTableFromGrid(grdView)
            Dim row As DataRow = dt.NewRow

            'Set one emty string to each column. (It should not be Nothing)
            ' This help keep Grid Data Table structure in XML
            For Each col As DataColumn In dt.Columns
                row(col.ColumnName) = ""
            Next

            dt.Rows.Add(row) ' Add empty Row ...
            Try ' Prevent crush application if previous data does not have this table (if dynamically add/delete new elements)
                DS.Tables.Remove(grdView.ID)
            Catch ex As Exception
            End Try
            DS.Tables.Add(dt)
        End If

        grdView.DataSource = DS.Tables(grdView.ID)
        grdView.DataBind()

    End Sub

    Public Sub initAllGridViewWithHandler(ByRef grd As GridView)

        AddHandler grd.RowEditing, AddressOf gridRowEditing
        AddHandler grd.RowCancelingEdit, AddressOf gridRowCancelingEdit
        AddHandler grd.RowDeleting, AddressOf gridRowDelete
        AddHandler grd.RowUpdating, AddressOf gridRowUpdaiting
        AddHandler grd.PageIndexChanging, AddressOf gridPageChanging


        For Each col As Object In grd.Columns
            If TypeOf (col) Is CommandField Then
                Dim cf As CommandField = CType(col, CommandField)
                'cf.ItemTemplate = New DynamicTemplatedGridViewHandler(ListItemType.Item, cf.HeaderText, "Command")
                'cf.EditItemTemplate = New DynamicTemplatedGridViewHandler(ListItemType.EditItem, cf.HeaderText, "Command")
            End If

            If TypeOf (col) Is TemplateField Then
                Dim tf As TemplateField = CType(col, TemplateField)
                tf.ItemTemplate = New DynamicTemplatedGridViewHandler(ListItemType.Item, tf.HeaderText, "String")
                tf.EditItemTemplate = New DynamicTemplatedGridViewHandler(ListItemType.EditItem, tf.HeaderText, "String")
            End If
        Next

    End Sub

    Private Sub gridPageChanging(sender As Object, e As GridViewPageEventArgs)
        Dim grd As GridView = CType(sender, GridView)
        grd.DataSource = DS.Tables(grd.ID)
        grd.PageIndex = e.NewPageIndex
        grd.DataBind()
    End Sub

    Private Sub gridRowUpdaiting(sender As Object, e As GridViewUpdateEventArgs)

        Dim grd As GridView = CType(sender, GridView)
        Dim indexDT As Integer = e.RowIndex + grd.PageIndex * grd.PageSize
        Dim rg As GridViewRow = grd.Rows(e.RowIndex) ' need to use only current page and row

        Dim dt As DataTable = DS.Tables(grd.ID)
        Dim dr As DataRow = dt.Rows(indexDT)

        For Each col As DataControlField In grd.Columns
            Dim name As String = col.HeaderText
            'Prevent saving information for columns like "Select", "Edit", ...
            If name <> "" Then
                dr.Item(name) = HttpContext.Current.Server.HtmlEncode(CType(rg.FindControl(name), TextBox).Text)
            End If

        Next

        ' Add empty row if current is last row
        If indexDT = dt.Rows.Count - 1 Or dt.Rows.Count = 0 Then
            Dim drNew As DataRow = dt.NewRow
            dt.Rows.Add(drNew)
        End If

        grd.EditIndex = -1
        grd.DataSource = dt
        grd.DataBind()
        grd.Focus()
    End Sub

    Private Sub gridRowDelete(sender As Object, e As GridViewDeleteEventArgs)
        Dim grd As GridView = CType(sender, GridView)
        Dim dt As DataTable = DS.Tables(grd.ID)
        Dim index As Integer = e.RowIndex + grd.PageIndex * grd.PageSize
        dt.Rows(index).Delete()

        If dt.Rows.Count = 0 Then
            Dim dr As DataRow = dt.NewRow
            dt.Rows.Add(dr)
        End If

        grd.EditIndex = -1
        grd.DataSource = DS.Tables(grd.ID)
        grd.DataBind()
        grd.Focus()
    End Sub

    Private Sub gridRowEditing(sender As Object, e As GridViewEditEventArgs)
        Dim grd As GridView = CType(sender, GridView)
        grd.EditIndex = e.NewEditIndex
        grd.DataSource = DS.Tables(grd.ID)
        grd.DataBind()
        grd.Focus()

    End Sub

    Private Sub gridRowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        Dim grd As GridView = CType(sender, GridView)
        grd.EditIndex = -1
        grd.DataSource = DS.Tables(grd.ID)
        grd.DataBind()
        grd.Focus()
    End Sub

#End Region
End Class
