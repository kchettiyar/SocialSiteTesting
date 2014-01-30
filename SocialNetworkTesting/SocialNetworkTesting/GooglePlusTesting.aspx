<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GooglePlusTesting.aspx.cs" Inherits="SocialNetworkTesting.GooglePlusTesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btn_GooglePlus" runat="server" Text="Connect To GooglePlus" 
                    onclick="btn_GooglePlus_Click" />
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td id="JSONData" runat="server">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
