<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Add_New.aspx.vb" Inherits="FastPrototypeWebsite.Add_New" %>

<%@ Register src="UserControls/MainControl.ascx" tagname="MainControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            height: 50%;
        }

        .auto-style3 {
            height: 50%;
            align-content: center
        }

        .auto-style5 {
            height: 62%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="auto-style1">
        <tr>
            <td class="auto-style2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style5">
                <uc1:MainControl ID="MainControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="auto-style3">
                    &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: center">
                &nbsp;
                    <asp:Button ID="btnResetAll" runat="server" Text="Reset All" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSave" runat="server" Text="Save All" Width="70px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <br />
            </td>
        </tr>
    </table>
</asp:Content>
