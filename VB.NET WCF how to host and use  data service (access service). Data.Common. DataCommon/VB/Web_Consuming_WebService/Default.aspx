<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Web_Consuming_WebService._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnSchema" runat="server" Text="Schema | Run" />
    
    </div>
        <p>
            <asp:TextBox ID="txtSQL" runat="server" Height="100px" TextMode="MultiLine" Width="100%">-- Uncomment somthing ... base on connection String in hosting apps ...
-- SELECT  * FROM INFORMATION_SCHEMA.COLUMNS -- for Ms SQL
-- SELECT  *  FROM SYSCAT.TABLES WHERE OWNERTYPE=&#39;U&#39;-- FOR DB2</asp:TextBox>
        </p>
        <p>
            <asp:TextBox ID="txtError" runat="server" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
        </p>
        <asp:GridView ID="grdSchema" runat="server" Width="100%">
        </asp:GridView>
    </form>
</body>
</html>
