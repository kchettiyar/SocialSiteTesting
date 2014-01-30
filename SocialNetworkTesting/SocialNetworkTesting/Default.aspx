<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SocialNetworkTesting._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table width="100%">
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btn_Linkedin" runat="server" Text="Connect To Linkedin" OnClick="btn_Linkedin_Click" />
                <br />
                <br />
                <br />
            </td>
            <td style="text-align: left;">
                <asp:Button ID="btn_Twitter" runat="server" Text="Connect To Twitter" 
                    onclick="btn_Twitter_Click" />
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td id="JSONData" runat="server" colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
