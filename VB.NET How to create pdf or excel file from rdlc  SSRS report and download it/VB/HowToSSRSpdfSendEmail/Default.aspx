<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="HowToSSRSpdfSendEmail._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <center>
        <div>
    
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Your Email Here."></asp:Label>
            <br />
            <asp:TextBox ID="txtEmail" runat="server" Width="298px"></asp:TextBox>
    
        <br />
        <br />
        <asp:Button ID="btnSend" runat="server" Text="SSRS to PDF attached to email ..." />
        <br />
        <br />
        <br />
    
    </div>

    </center>
    </form>
</body>
</html>
