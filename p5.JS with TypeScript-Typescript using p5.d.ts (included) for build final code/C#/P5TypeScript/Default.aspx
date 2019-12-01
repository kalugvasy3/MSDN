<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="P5TypeScript.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="JavaScript/p5.js">  </script>
    <script type="text/javascript" src="main.js">  </script>
   
    <link rel="icon" href="data:;base64,iVBORw0KGgo="/>
    
    <style type="text/css">
        html, body {
            padding: 0;
            margin: 0;
            overflow: hidden;
        }

        canvas {
            width: 100%;
            height: 100%;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        
        <table style="width: 100%" id="tbl">
            <tr>
                <td style="width: 50%">
                    <div id="morph1" style="width: 99%"></div>  <!-- if you setup 100% size will not be working with reduse size -->
                </td>
        
                <td style="width: 50%">
                    <div id="morph2" style="width: 99%"></div>
                </td>
            </tr>
        </table>     

    </form>
</body>

</html>
