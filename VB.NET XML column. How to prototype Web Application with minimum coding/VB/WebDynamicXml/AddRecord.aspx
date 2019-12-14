<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddRecord.aspx.vb" Inherits="WebDynamicXml.AddRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
  <asp:Panel ID="pnlMain" runat="server">

     <table border="0"   align="Center" style="border-color: #FFFFFF" >

        <tr>
            <td style="border-color: #FFFFFF; text-align: center; height: 46px;" colspan="3" >
                <asp:RadioButton ID="rbSeriousInjury" runat="server" GroupName="grReporting" Text="Serious Injury " OnCheckedChanged="rbSeriousInjury_CheckedChanged" />
                <asp:RadioButton ID="rbMinorInjury" runat="server" GroupName="grReporting" Text="Minor Injury" OnCheckedChanged="rbMinorInjury_CheckedChanged" />
                &nbsp;
                <asp:TextBox ID="Injury" runat="server" Visible="False" Width="55px"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td style="border-color: #FFFFFF;" >&nbsp;</td>
            <td style="border-color: #FFFFFF;" >
                &nbsp;</td>
            <td style="border-color: #FFFFFF;" >
                <asp:Label ID="lblNA" runat="server" Font-Bold="True" Text="Please type &quot;N/A&quot; if information is not available."></asp:Label>
            </td>
        </tr>

        <tr>
            <td style="border-color: #FFFFFF;" >&nbsp;</td>
            <td style="border-color: #FFFFFF;" >
                <asp:Label ID="lblDateOfIncident" runat="server" Text="Date  of Incident"></asp:Label>
            &nbsp;
                <asp:Label ID="lblMMDDYYYY_" runat="server" Text="(mm/dd/yyyy)" Font-Bold="True"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;" >
                <asp:TextBox ID="Date_Of_Incident" runat="server" ToolTip="Date Of Incident" Width="130px" MaxLength="10"  ClientIDMode="Static" TextMode="Date" ValidationGroup="Group1"></asp:TextBox>
                </td>
        </tr>

        <tr>
            <td style="border-color: #FFFFFF;" >&nbsp;</td>
            <td style="border-color: #FFFFFF;" >
                <asp:Label ID="lblTimeOfIncident" runat="server" Text="Time  of Incident"></asp:Label>
            &nbsp;
                <asp:Label ID="lblTime" runat="server" Text="(hh:mm AM/PM)" Font-Bold="True"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;" >
                <asp:TextBox ID="Time_Of_Incident" runat="server" ToolTip="Time Of Incident" Width="130px" MaxLength="10"  ClientIDMode="Static" TextMode="Time" ValidationGroup="Group1"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td style="border-color: #FFFFFF;" ></td>
            <td style="border-color: #FFFFFF;" >
                <asp:Label ID="lblNameOfProvider" runat="server" Text="Name Of Provider"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;" >
                <asp:TextBox ID="Name_Of_Provider" runat="server" ToolTip="Name Of Provider" Width="348px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblPhoneNumber" runat="server" Text="Phone Number"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Phone_Number" runat="server" TextMode="Phone" ToolTip="Phone Number" Width="130px" MaxLength="14"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Address" runat="server" ToolTip="Address" Width="348px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF"></td>
            <td style="border-color: #FFFFFF">
                <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF">
                <asp:TextBox ID="City" runat="server" ToolTip="City" Width="348px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblCounty" runat="server" Text="County"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="County" runat="server" ToolTip="County" Width="130px" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                &nbsp;</td>
            <td style="border-color: #FFFFFF;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblChildFirstName" runat="server" Text="Child First Name"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Child_First_Name" runat="server" ToolTip="Child First Name" Width="348px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblChildLastName" runat="server" Text="Child Last Name"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Child_Last_Name" runat="server" ToolTip="Child Last Name" Width="348px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF; height: 46px;"></td>
            <td style="border-color: #FFFFFF; height: 46px;">
                <asp:Label ID="lblChildDOB" runat="server" Text="Child DOB"></asp:Label>
            &nbsp;<asp:Label ID="lblMMDDYYYY" runat="server" Text="(mm/dd/yyyy)" Font-Bold="True"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF; height: 46px;">
                <asp:TextBox ID="Child_DOB" runat="server" ToolTip="Child DOB" Width="130px" MaxLength="10" TextMode="Date" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td   style="border-color: #FFFFFF;"></td>
            <td   style="border-color: #FFFFFF;">
                <asp:Label ID="lblChildSex" runat="server" Text="Child SEX"></asp:Label>
            &nbsp;
                <asp:Label ID="lblChildMF" runat="server" Text=" (M/F)" Font-Bold="True"></asp:Label>
            </td>
            <td   style="border-color: #FFFFFF;">
                <asp:TextBox ID="Child_Sex" runat="server" ToolTip="SEX (M/F)" Width="29px" MaxLength="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                &nbsp;</td>
            <td style="border-color: #FFFFFF;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblParentFirstName" runat="server" Text="Parent First Name"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Parent_First_Name" runat="server" ToolTip="Parent First Name" Width="348px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblParentLastName" runat="server" Text="Parent Last Name"></asp:Label>
            </td>
            <td style="border-color: #FFFFFF;">
                <asp:TextBox ID="Parent_Last_Name" runat="server" ToolTip="Parent Last Name" Width="348px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;"></td>
            <td style="border-color: #FFFFFF;">
                <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
            </td>
            <td  style="border-color: #FFFFFF;">
                <asp:TextBox ID="Description" runat="server" Height="100px" MaxLength="2000" TextMode="MultiLine" ToolTip="Description" Width="348px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">&nbsp;</td>
            <td style="border-color: #FFFFFF;">
                <asp:Button ID="btnSend" runat="server" Text="Send" Width="120px" ValidationGroup="Group1" />
            </td>
        </tr>
        </table>

    </asp:Panel>
    </form>
</body>
</html>
