<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TypeScriptSessionExpier.Default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

    <br />
    <br />

    <asp:Label ID="lblMainPage" runat="server" Text="Main Page"
        Font-Bold="True"  Font-Size="Large">
    </asp:Label>

    <br />

    <div  class="center">
        <table style="width: 100%; height:400px; background-color: #CCCCCC; text-align: center;" >
            <tr>
                <td>1&nbsp;</td>
                <td>2&nbsp;</td>
                <td>3&nbsp;</td>
                <td>4&nbsp;</td>
                <td>5&nbsp;</td>
            </tr>
            <tr>
                <td>2&nbsp;</td>
                <td>3&nbsp;</td>
                <td>4&nbsp;</td>
                <td>5&nbsp;</td>
                <td>1&nbsp;</td>
            </tr>
            <tr>
                <td>3&nbsp;</td>
                <td>4&nbsp;</td>
                <td>5&nbsp;</td>
                <td>1&nbsp;</td>
                <td>2&nbsp;</td>
            </tr>
            <tr>
                <td>4&nbsp;</td>
                <td>5&nbsp;</td>
                <td>1&nbsp;</td>
                <td>2&nbsp;</td>
                <td>3&nbsp;</td>
            </tr>
            <tr>
                <td>5&nbsp;</td>
                <td>1&nbsp;</td>
                <td>2&nbsp;</td>
                <td>3&nbsp;</td>
                <td>4&nbsp;</td>
            </tr>
        </table>
    </div>

    <br />
    <br />
    <br />
</asp:Content>
