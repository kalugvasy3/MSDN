<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="WebDataCommon.TEST" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td>&nbsp;</td>
                <td style="width: 33%; text-align: center;">
                    <asp:Button ID="btnA" runat="server" OnClick="btnA_Click" Text="DB2 (Default)" ToolTip="Default" Width="100px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 33%; text-align: center;">
                    <asp:Button ID="btnB" runat="server" OnClick="btnB_Click" Text="MS SQL (dbB)" Width="100px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 33%; text-align: center;">
                    <asp:Button ID="btnC" runat="server" OnClick="btnC_Click" Text="MySQL (dbC)" Width="100px" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 33%; text-align: center;">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 33%; text-align: center;">
                    <asp:GridView ID="grdView" runat="server" Width="100%">
                    </asp:GridView>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    <div>
    
    </div>
    </form>
</body>
</html>
