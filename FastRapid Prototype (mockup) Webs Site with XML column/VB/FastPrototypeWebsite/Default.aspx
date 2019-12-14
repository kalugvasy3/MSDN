<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.vb" Inherits="FastPrototypeWebsite._Default" %>

<%@ Register Src="UserControls/MainControl.ascx" TagName="MainControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="auto-style1">
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView ID="grdAll" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="PK_INDEX" HeaderText="PK_INDEX" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="DATE" HeaderText="DATE" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="XML" HeaderText="XML" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;
                <asp:Button ID="btnAddNew" runat="server" Text="New" Width="70px" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Panel ID="pnlResult" runat="server" Visible="False">

                    <uc1:MainControl ID="MainControl1" runat="server" />

                    <div style="text-align: center">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" />
                        &nbsp;
                        <asp:Label ID="lblCurrent" runat="server" Text="Current selected record is "></asp:Label>
                    </div>

                    <br />
                </asp:Panel>

            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
