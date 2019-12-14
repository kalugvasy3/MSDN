<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FirstRun.aspx.vb" Inherits="FastPrototypeWebsite.FirstRun" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="txtCreateTable" runat="server" Height="400px" TextMode="MultiLine" Width="100%">CREATE TABLE &quot;FastPrototypeWebSite&quot;(&quot;PK_INDEX&quot; int IDENTITY(1,1) PRIMARY KEY , &quot;XML&quot;  xml    NOT NULL,  &quot;DATE&quot; DATE   NOT NULL) GO ; </asp:TextBox>
    
    </div>
        <asp:Button ID="btnCreate" runat="server" Text="Create" Width="135px" />
    </form>
</body>
</html>
