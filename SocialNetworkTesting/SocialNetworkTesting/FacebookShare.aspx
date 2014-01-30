<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacebookShare.aspx.cs"
    Inherits="SocialNetworkTesting.FacebookShare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function _Test() {
            var ImgSrc = document.getElementById("imgHref").href;
            var EncodeURL = encodeURIComponent(ImgSrc);
            return EncodeURL;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="https://fbcdn-sphotos-c-a.akamaihd.net/hphotos-ak-prn1/936472_506498129405530_1605989682_n.jpg"
            id="imgHref"></a><a href="#" onclick="
    window.open(
      'https://www.facebook.com/sharer/sharer.php?u='+_Test(), 
      'facebook-share-dialog', 
      'width=626,height=436'); 
    return false;">Share on Facebook </a>
    </div>
    </form>
</body>
</html>