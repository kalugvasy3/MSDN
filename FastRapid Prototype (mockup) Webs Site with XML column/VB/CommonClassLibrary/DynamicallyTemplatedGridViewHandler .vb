' http://developers.de/blogs/nadine_storandt/archive/2007/07/10/dynamically-create-edititemtemplate-at-runtime-in-a-gridview-control.aspx
' http://aspalliance.com/articleViewer.aspx?aId=1125&pId=-1
'https://www.aspsnippets.com/Articles/Dynamically-add-BoundField-and-TemplateField-Columns-to-GridView-in-ASPNet.aspx
' http://converter.telerik.com/
'http://www.developer.com/net/asp/article.php/3633561/Build-a-Nested-GridView-Control-with-ASPNET.htm

Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports CommonClassLibrary

' Base on this example http://aspalliance.com/articleViewer.aspx?aId=1125&pId=-1

Public Class DynamicTemplatedGridViewHandler
    Implements ITemplate

    Dim ItemType As ListItemType
    Dim FieldName As String
    Dim InfoType As String

    Public Sub New(ByVal Item_type As ListItemType, ByVal field_name As String, ByVal Info_Type As String)
        ItemType = Item_type
        FieldName = field_name
        InfoType = Info_Type
    End Sub

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn

        '    We DO NOT NEED IN THIS approch Header 

        'If ItemType = ListItemType.Header Then
        '    Dim header_ltrl As Literal = New Literal()
        '    header_ltrl.Text = "<b>" + FieldName + "</b>"
        '    container.Controls.Add(header_ltrl)
        'End If


        If ItemType = ListItemType.Item Then

            If InfoType = "Button" Then
                '    We DO NOT NEED IN THIS approch COMMAND 

                'Dim lnk_button = New LinkButton
                'lnk_button.ID = "edit_button"
                'lnk_button.CommandName = "Edit"
                'AddHandler lnk_button.Click, AddressOf EditButtonClickg

            Else
                Dim field_lbl As Label = New Label()
                field_lbl.ID = FieldName '.Replace("txt", "lbl")
                ' field_lbl.Text = String.Empty
                AddHandler field_lbl.DataBinding, AddressOf OnDataBinding
                container.Controls.Add(field_lbl)
            End If



        End If

        If ItemType = ListItemType.EditItem Then

            If InfoType = "Button" Then
                '    We DO NOT NEED IN THIS approch COMMAND 

                'Dim lnk_button = New LinkButton
                'lnk_button.ID = "update_button"
                'lnk_button.CommandName = "Update"
                'AddHandler lnk_button.Click, AddressOf EditButtonClickg

            Else
                Dim field_txtbox As TextBox = New TextBox()
                field_txtbox.ID = FieldName
                ' field_txtbox.Text = String.Empty
                AddHandler field_txtbox.DataBinding, AddressOf OnDataBinding
                container.Controls.Add(field_txtbox)
            End If
        End If

    End Sub

    'Private Sub EditButtonClickg(sender As Object, e As EventArgs)
    '    We DO NOT NEED IN THIS approch COMMAND 
    '    Throw New NotImplementedException()
    'End Sub

    Private Sub OnDataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim bound_value_obj As Object = Nothing
        Dim ctrl As Control = CType(sender, Control)
        Dim data_item_container As IDataItemContainer = CType(ctrl.NamingContainer, IDataItemContainer)

        Try
            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName)
        Catch ex As Exception
            Exit Sub
        End Try


        If TypeOf (sender) Is TextBox Then
            Try
                CType(sender, TextBox).Text = HttpContext.Current.Server.HtmlDecode(bound_value_obj)
            Catch ex As Exception
                CType(sender, TextBox).Text = ""
            End Try

        ElseIf TypeOf (sender) Is Label Then
            Try
                CType(sender, Label).Text = bound_value_obj
            Catch ex As Exception
                CType(sender, Label).Text = ""
            End Try
        End If

        'SharedValue Value  DS 

    End Sub



End Class


