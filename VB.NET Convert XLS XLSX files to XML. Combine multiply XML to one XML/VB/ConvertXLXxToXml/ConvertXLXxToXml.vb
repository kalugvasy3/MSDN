''' Download and install ... first ... Microsoft Access Database Engine 
''' http://www.microsoft.com/en-us/download/details.aspx?id=13255       
''' http://www.microsoft.com/en-us/download/confirmation.aspx?id=23734 


Option Strict On
Option Explicit On

Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Net
Imports System.Data
Imports Excel = Microsoft.Office.Interop.Excel



Public Class ConvertXLXxToXml

    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Public WithEvents btnOpenFile As System.Windows.Forms.Button
    Public WithEvents DG As System.Windows.Forms.DataGrid
    Friend WithEvents PrgBar As System.Windows.Forms.ProgressBar
    Friend WithEvents OpenXMLFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents rbXml As System.Windows.Forms.RadioButton
    Friend WithEvents rbExcel As System.Windows.Forms.RadioButton
    Public WithEvents btnSaveAsXML As System.Windows.Forms.Button
    Friend WithEvents SaveTxtFileDialog As System.Windows.Forms.SaveFileDialog

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnOpenFile = New System.Windows.Forms.Button()
        Me.OpenXMLFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SaveTxtFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.DG = New System.Windows.Forms.DataGrid()
        Me.PrgBar = New System.Windows.Forms.ProgressBar()
        Me.rbXml = New System.Windows.Forms.RadioButton()
        Me.rbExcel = New System.Windows.Forms.RadioButton()
        Me.btnSaveAsXML = New System.Windows.Forms.Button()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOpenFile
        '
        Me.btnOpenFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOpenFile.Location = New System.Drawing.Point(12, 312)
        Me.btnOpenFile.Name = "btnOpenFile"
        Me.btnOpenFile.Size = New System.Drawing.Size(173, 23)
        Me.btnOpenFile.TabIndex = 0
        Me.btnOpenFile.Text = "Open/Find  ... file"
        '
        'OpenXMLFileDialog
        '
        Me.OpenXMLFileDialog.Filter = "Xml files (*.xml)|*.xml| XLS files (*.xls)|*.xls"
        Me.OpenXMLFileDialog.Multiselect = True
        '
        'SaveTxtFileDialog
        '
        Me.SaveTxtFileDialog.DefaultExt = "txt"
        '
        'DG
        '
        Me.DG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DG.CaptionText = "XML"
        Me.DG.DataMember = ""
        Me.DG.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DG.Location = New System.Drawing.Point(4, -6)
        Me.DG.Name = "DG"
        Me.DG.ReadOnly = True
        Me.DG.RowHeaderWidth = 0
        Me.DG.Size = New System.Drawing.Size(755, 312)
        Me.DG.TabIndex = 1
        '
        'PrgBar
        '
        Me.PrgBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrgBar.Location = New System.Drawing.Point(4, 346)
        Me.PrgBar.Name = "PrgBar"
        Me.PrgBar.Size = New System.Drawing.Size(755, 16)
        Me.PrgBar.Step = 5
        Me.PrgBar.TabIndex = 4
        '
        'rbXml
        '
        Me.rbXml.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbXml.AutoSize = True
        Me.rbXml.Checked = True
        Me.rbXml.Location = New System.Drawing.Point(211, 317)
        Me.rbXml.Name = "rbXml"
        Me.rbXml.Size = New System.Drawing.Size(42, 17)
        Me.rbXml.TabIndex = 5
        Me.rbXml.TabStop = True
        Me.rbXml.Text = "Xml"
        Me.rbXml.UseVisualStyleBackColor = True
        '
        'rbExcel
        '
        Me.rbExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbExcel.AutoSize = True
        Me.rbExcel.Location = New System.Drawing.Point(274, 317)
        Me.rbExcel.Name = "rbExcel"
        Me.rbExcel.Size = New System.Drawing.Size(51, 17)
        Me.rbExcel.TabIndex = 6
        Me.rbExcel.Text = "Excel"
        Me.rbExcel.UseVisualStyleBackColor = True
        '
        'btnSaveAsXML
        '
        Me.btnSaveAsXML.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAsXML.Enabled = False
        Me.btnSaveAsXML.Location = New System.Drawing.Point(414, 311)
        Me.btnSaveAsXML.Name = "btnSaveAsXML"
        Me.btnSaveAsXML.Size = New System.Drawing.Size(173, 23)
        Me.btnSaveAsXML.TabIndex = 8
        Me.btnSaveAsXML.Text = "Save As XML"
        '
        'XmlOpenAsDS
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(763, 360)
        Me.Controls.Add(Me.btnSaveAsXML)
        Me.Controls.Add(Me.rbExcel)
        Me.Controls.Add(Me.rbXml)
        Me.Controls.Add(Me.PrgBar)
        Me.Controls.Add(Me.DG)
        Me.Controls.Add(Me.btnOpenFile)
        Me.MinimumSize = New System.Drawing.Size(750, 333)
        Me.Name = "XmlOpenAsDS"
        Me.Text = "XML Open"
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Dim DtSt As DataSet
    Dim DtStGr As DataSet
    Dim pathString As String()
    Dim strTxtName As String = ""

    Dim findData As FileStream


    Private Sub btnOpenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFile.Click

        Me.OpenXMLFileDialog.FileName = ""

        If rbXml.Checked Then

            Me.OpenXMLFileDialog.Filter = "XML|*.xml"
            Me.OpenXMLFileDialog.Multiselect = True
            openFileXml()

        Else
            'Me.OpenXMLFileDialog.Filter = "XLS|*.xls"
            Me.OpenXMLFileDialog.Filter = "XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx"
            Me.OpenXMLFileDialog.Multiselect = False
            openFileXlsXlsx()

        End If


    End Sub

    Private Sub openFileXml()


        'OPEN DIALOG (choose XML - file)

        If OpenXMLFileDialog.ShowDialog() = DialogResult.Cancel Or OpenXMLFileDialog.FileName = "" Then
            DG.DataSource = Nothing
            btnSaveAsXML.Enabled = False
            Exit Sub
        End If

        btnSaveAsXML.Enabled = True
        pathString = OpenXMLFileDialog.FileNames

        Dim pathXls As String

        DtStGr = New DataSet

        For Each pathXls In pathString
            DtSt = New DataSet
            xmlLoad(pathXls)
            DtStGr.Merge(DtSt)

        Next
        DtStGr.DataSetName = "xml"
        DtStGr.AcceptChanges()

        Dim tstyle As New DataGridTableStyle
        tstyle.AllowSorting = True
        tstyle.RowHeaderWidth = 0

        DG.TableStyles.Add(tstyle)

        DG.NavigateBack()  ' If need to re-open new file it help does not lose navigation control
        DG.NavigateBack()
        DG.DataSource = DtStGr

    End Sub


    Private Sub openFileXlsXlsx()

        If OpenXMLFileDialog.ShowDialog() = DialogResult.Cancel Or OpenXMLFileDialog.FileName = "" Then
            DG.DataSource = Nothing
            btnSaveAsXML.Enabled = False
            Exit Sub
        End If

        btnSaveAsXML.Enabled = True
        pathString = OpenXMLFileDialog.FileNames
        Dim extention As String = Path.GetExtension(pathString(0))
        Dim nameFile As String = Path.GetFileNameWithoutExtension(pathString(0))

        Dim MyConnection As System.Data.OleDb.OleDbConnection = Nothing
        Dim MyCommand As System.Data.OleDb.OleDbDataAdapter

        Try

            If extention = ".xls" Then
                MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathString(0) + ";Extended Properties=Excel 8.0;")
            Else
                MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathString(0) + ";Extended Properties=""Excel 12.0 Xml;HDR=YES;"" ")
            End If

            MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection)
            MyCommand.TableMappings.Add(nameFile, nameFile)

            DtStGr = New System.Data.DataSet()
            DtStGr.DataSetName = "xml"
            DG.CaptionText = pathString(0)

            MyCommand.Fill(DtStGr)

            DtStGr.Tables(0).TableName = nameFile

            DtStGr.AcceptChanges()
            MyConnection.Close()

            Dim tstyle As New DataGridTableStyle
            tstyle.AllowSorting = True
            tstyle.RowHeaderWidth = 0

            DG.TableStyles.Add(tstyle)

            DG.NavigateBack()  ' If need to re-open new file it help does not lose navigation control
            DG.NavigateBack()
            DG.DataSource = DtStGr

        Catch ex As Exception
            'Microsoft Access Database Engine 2010 Redistributable (x32) 
            Dim strM As String = "Please Install two Packages (for open Excel):" + vbNewLine
            strM += "http://www.microsoft.com/en-us/download/details.aspx?id=13255" + vbNewLine
            strM += "http://www.microsoft.com/en-us/download/confirmation.aspx?id=23734 "

            MessageBox.Show(strM, "ConvertXLXxToXml", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Finally
            MyConnection.Close()
        End Try
    End Sub


    Private Sub xmlLoad(ByVal pathXML As String)
        Dim strPathName As String

        Try
            findData = New FileStream(pathXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            DtSt.ReadXml(findData)
            findData.Close()

            strPathName = CStr(pathXML.Split("\"c).GetValue(pathXML.Split("\"c).Length - 1))
            'strTxtName = CStr(strPathName.Split("."c).GetValue(0)) + ".txt"

        Catch
            'MessageBox.Show("Please check File/s : """ & pathXML & """ AND """ & strPathName & """", "XML_GED", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub btnSaveAsXML_Click(sender As Object, e As EventArgs) Handles btnSaveAsXML.Click
        Dim myStream As Stream
        Dim saveFileDialog As New SaveFileDialog()

        saveFileDialog.Filter = "XML files (*.xml)|*.xml"
        saveFileDialog.FilterIndex = 1
        saveFileDialog.RestoreDirectory = True

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog.OpenFile()
            If (myStream IsNot Nothing) Then
                DtStGr.WriteXml(myStream)
                myStream.Close()
            End If
        End If

    End Sub



    '' How to create XmlDocument 

    'Private Function WriteTo_H(ByVal XMLdata As XmlDocument) As Integer
    '    Dim sw As StreamWriter

    '    Dim intNumbFiles As Integer = XMLdata.GetElementsByTagName("XMLDocument").Count
    '    Dim intTmp As Integer = 0

    '    For intTmp = 0 To intNumbFiles - 1
    '        Dim strName As String = XMLdata.GetElementsByTagName("Name").Item(intTmp).InnerText
    '        If File.Exists(strName) Then
    '            File.Delete(strName)
    '        End If
    '        sw = File.CreateText(strName)

    '        sw.Write("<?xml version=""1.0"" encoding=""utf-8"" ?> ")  '<-- preambul
    '        sw.Write(XMLdata.GetElementsByTagName("XMLDocument").Item(intTmp).InnerXml)
    '        sw.Close()
    '    Next
    '    Return intNumbFiles
    'End Function



End Class

