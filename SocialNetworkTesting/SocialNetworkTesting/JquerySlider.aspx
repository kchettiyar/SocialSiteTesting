<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JquerySlider.aspx.cs" Inherits="SocialNetworkTesting.JquerySlider" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Text Changes with Sliding Effect </title>
    <script type="text/javascript" src="Scripts/jquery.js"></script>
    <script type="text/javascript" src="Scripts/jqfade.js"></script>
    <script type="text/javascript">
        $(document).ready(
				function () {
				    $('#news').innerfade({
				        animationtype: 'fade',
				        speed: 1000,
				        timeout: 2000,
				        type: 'random'
				    });
				});
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul id="news">
            <li>1.Exercise your brain. Brains, like bodies, need exercise to keep fit. If you don't
                exercise your brain, it will get flabby and useless. Exercise your brain by reading
                a lot. </li>
            <li>2.Read as much as you can about everything possible. Books exercise your brain,
                provide inspiration and fill you with information that allows you to make creative
                connections easily. </li>
            <li>3.Don't do drugs. People on drugs think they are creative. To everyone else, they
                seem like people on drugs. </li>
        </ul>
    </div>
    </form>
</body>
</html>
