<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MainControl.ascx.vb" Inherits="FastPrototypeWebsite.MainControl" %>
<style type="text/css">
    .auto-style1 {
        width: 100%;
    }
</style>

<table class="auto-style1" id="tblMain">
    <tr>
        <td class="auto-style2">
            <asp:Panel ID="pnlDemographic" runat="server" Height="100%" BorderColor="#9900CC" BackColor="#CCCCCC">
                <br />
                <table class="auto-style1">
                    <tr>
                        <td>
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td rowspan="8" style="width: 50%; vertical-align: middle; text-align: left; height: 100%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblLastName" runat="server" Text="LastName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDOB" runat="server" Text="DOB" Width="100%"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="DateTime" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStateNull" runat="server" Width="100px">
                                <asp:ListItem> -Select-</asp:ListItem>
                                <asp:ListItem> Alabama </asp:ListItem>
                                <asp:ListItem>Alaska</asp:ListItem>
                                <asp:ListItem>Arizona</asp:ListItem>
                                <asp:ListItem> Arkansas </asp:ListItem>
                                <asp:ListItem>California</asp:ListItem>
                                <asp:ListItem>Colorado</asp:ListItem>
                                <asp:ListItem> Connecticut </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnResetDemographic" runat="server" Text="Reset" Width="70px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbTest" runat="server" Text="Check" />
                            &nbsp;
                            </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td class="auto-style5">
            <asp:Panel ID="pnlSkills" runat="server" BorderColor="#9900CC" BackColor="Silver">
                <br />
                <br />
                &nbsp;<asp:ListBox ID="listOfSkills" runat="server" Height="200px" SelectionMode="Multiple" Width="150px">
                    <asp:ListItem>VB.Net</asp:ListItem>
                    <asp:ListItem>C#</asp:ListItem>
                    <asp:ListItem>F#</asp:ListItem>
                    <asp:ListItem>C</asp:ListItem>
                    <asp:ListItem>ADA</asp:ListItem>
                    <asp:ListItem>Fortran</asp:ListItem>
                    <asp:ListItem>Pascal</asp:ListItem>
                    <asp:ListItem>Delphi</asp:ListItem>
                    <asp:ListItem>Assembler</asp:ListItem>
                    <asp:ListItem>COBOL</asp:ListItem>
                    <asp:ListItem>Java</asp:ListItem>
                    <asp:ListItem>Lisp</asp:ListItem>
                    <asp:ListItem>Haskel</asp:ListItem>
                    <asp:ListItem>Java Script</asp:ListItem>
                    <asp:ListItem>MatLab</asp:ListItem>
                </asp:ListBox>
                &nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="cbTest1" runat="server" Height="24px" Text="Check1" />
                <br />
                <br />
                &nbsp;<asp:Button ID="btnResetSkills" runat="server" Text="Reset" Width="70px" />
                <br />
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td class="auto-style3">&nbsp;
 
                <br />

            <asp:GridView ID="grdJob" runat="server" AllowPaging="True" AutoGenerateColumns="False" HorizontalAlign="Center" PageSize="5" Width="94%" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CommandField>

                    <asp:TemplateField HeaderText="ID"></asp:TemplateField>

                    <asp:TemplateField HeaderText="Organization"></asp:TemplateField>

                    <asp:TemplateField HeaderText="BeginDate"></asp:TemplateField>

                    <asp:TemplateField HeaderText="EndDate"></asp:TemplateField>

                </Columns>
                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#330099" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                <SortedAscendingCellStyle BackColor="#FEFCEB" />
                <SortedAscendingHeaderStyle BackColor="#AF0101" />
                <SortedDescendingCellStyle BackColor="#F6F0C0" />
                <SortedDescendingHeaderStyle BackColor="#7E0000" />
            </asp:GridView>
            <br />

            <asp:GridView ID="grdTest" runat="server" AllowPaging="True" AutoGenerateColumns="False" HorizontalAlign="Center" PageSize="5" Width="94%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center">

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CommandField>

                    <asp:TemplateField HeaderText="Test0"></asp:TemplateField>

                    <asp:TemplateField HeaderText="Test1"></asp:TemplateField>

                    <asp:TemplateField HeaderText="Test2"></asp:TemplateField>

                    <asp:TemplateField HeaderText="YYY"></asp:TemplateField>

                </Columns>

                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />

            </asp:GridView>

            <br />

            <br />

            <br />
            &nbsp;<br />

        </td>
    </tr>
</table>

