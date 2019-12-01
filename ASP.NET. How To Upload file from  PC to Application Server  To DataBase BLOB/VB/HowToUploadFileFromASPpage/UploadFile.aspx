<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadFile.aspx.vb" Inherits="HowToUploadFileFromASPpage.UploadFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Add &quot;FileUpload&quot; control from&nbsp; &quot;Toolbox&quot;<br />
        <br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblAttention" runat="server" Text="Attention!"></asp:Label>
        <br />
        <br />
        Example How to save file to memory or DataBase BLOB location.<br />
        <br />
        <asp:Button ID="btnSaveToMemory" runat="server" Text="Save To Memory and DB" Width="216px" />
        <br />
        <br />
        Example How to Save To Server Folder.<br />
        <br />
        <asp:Button ID="btnSaveOnServerForlder" runat="server" Text="Save On Server Forlder" Width="214px" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        <br />
        <br />
        How to Read file from Memory or&nbsp; from DB BLOB object.<br />
&nbsp;<br />
        <asp:Button ID="btnReadFromMemoryDB" runat="server" Text="Read From Memory Or DB" Width="217px" />
        </div>
        &nbsp;</form>
    <p>
        &nbsp;</p>
</body>
</html>
