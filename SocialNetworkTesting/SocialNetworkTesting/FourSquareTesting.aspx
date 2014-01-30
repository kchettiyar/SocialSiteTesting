<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FourSquareTesting.aspx.cs" Inherits="SocialNetworkTesting.FourSquareTesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btn_FourSquare" runat="server" Text="Connect To FourSquare" 
                    onclick="btn_FourSquare_Click" />
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
