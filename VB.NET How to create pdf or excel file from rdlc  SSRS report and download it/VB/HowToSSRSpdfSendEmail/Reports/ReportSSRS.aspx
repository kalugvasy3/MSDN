﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportSSRS.aspx.vb" Inherits="HowToSSRSpdfSendEmail.ReportSSRS" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="rv" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
