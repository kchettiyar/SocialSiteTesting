<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="APITesting.aspx.cs" Inherits="SocialNetworkTesting.APITesting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $('#fb-btn-rego').click(function (e) {
            e.stopPropagation();
            e.preventDefault();

            var popUpWin = _windowPopUp($(this).attr("href"), 640, 480);

            $(popUpWin).bind("popup.close", null, function () {
                alert("closing");
            })

            $(popUpWin).load(function () {
                popUpWin.close();
                parent.window.location = "/APITesting";
            })
            return false;
        });

        var _iframePopUp = function (url, w, h) {
            var divIframe = document.createElement("div");
            var left = screen.width ? (screen.width - w) / 2 : 0;
            var top = screen.height ? (screen.height - h) / 2 : 0;
            $(divIframe).css({ "position": "absolute", "width": w + "px", "height": h + "px", "top": top + "px", "left": left + "px" })
            var iframe = document.createElement("iframe");
            $("body").append(divIframe);

            $(iframe).attr({ "scrolling": 0, "frameborder": 0, "src": url }).css({ "width": "100%", "height": "100%" }).appendTo(divIframe)
        }

        var _windowPopUp = function (url, w, h) {
            var left = screen.width ? (screen.width - w) / 2 : 0;
            var top = screen.height ? (screen.height - h) / 2 : 0;
            var winSettings = 'height=' + h + ' ,width=' + w + ' ,top=' + top + ' ,left=' + left + ' ,status=0,toolbar=0,resizable=0,scrollbars=0,directories=0,location=1';
            var popupWin = window.open(url, 'fbauth', winSettings, null);
            return popupWin;
        }

        var _setRegoForm = function (obj) {
            $("#pp_rego_email").val(obj.Email);
            $("#pp_rego_dob_date").val(obj.DOBDate);
            $("#pp_rego_dob_month").val(obj.DOBMonth);
            $("#pp_rego_dob_year").val(obj.DOBYear);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <a id="fb-btn-rego" target="_blank" href="/Testing.aspx">Connect your facebook account ›</a>
        
    </div>
</asp:Content>
