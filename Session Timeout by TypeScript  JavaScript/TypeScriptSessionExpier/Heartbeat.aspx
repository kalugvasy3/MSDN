<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Heartbeat.aspx.cs" Inherits="TypeScriptSessionExpier.Heartbeat" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <div runat="server" id="divDateTime"></div>
    <!--Response.Write(DateTime.Now.ToLongTimeString() !!! .ASPX  Response does not work with IE11
        MUST USE runat="server" 
        -->
    <script>
        var d = new Date().toLocaleTimeString();
        document.getElementById("divDateTime").innerHTML = d;
    </script>
</body>
</html>
