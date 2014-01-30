<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FreeBase.aspx.cs" Inherits="SocialNetworkTesting.FreeBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btn_FreeBase" runat="server" Text="Connect To FreeBase" OnClick="btn_FreeBase_Click" />
                <br />
                <br />
                <br />
            </td>
            <td>
                <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td id="JSONData" runat="server" colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
