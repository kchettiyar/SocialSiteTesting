<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Testing.aspx.cs" Inherits="SocialNetworkTesting.Testing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.5.1.js"></script>
    <script type="text/javascript">

        var popupInstance = null;

        function createCrossBrowserPopup() {
            //            alert("creating");
            return new _popup();
        }

        function _popup() {
            this.wnd = null;
            this.hiddenFrame = null; //workaround for IE 6 - with divs and select (divs are appearing behind select elements)
            this.showHiddenFrame = showHiddenFrame;
            this.displayPopupContainer = displayPopupContainer;
            this.html = "";
            this.setHTML = _setHTML;
            this.show = _show;
            this.hide = _hide;
            this.triggerElementId = null;
            this.blur = null;
        }

        function _setHTML(html, event) {
            this.html = html;
            if (this.wnd != null) {
                this.wnd.html(this.html);
            }

            if (event != null)
                stopEventPropagation(event, null);
        }

        /// leftOffset: the left offset of the popup
        /// topOffset: the top offset of the popup
        /// width: the width of the popup
        /// height: the height of the popup
        /// element: the element which causes the popup to be opened
        /// event: the event which cased the popup to be opened
        function _show(leftOffset, topOffset, width, height, element, event) {

            // Hide previous popup if existed
            if (popupInstance != null && element.id != popupInstance.triggerElementId) {
                popupInstance.hide();
                return;
            }

            // if popupInstance != null this means that the same popup was called, makes no sense to create it again
            if (popupInstance == null) {
                leftOffset = leftOffset != null ? leftOffset : 0;
                topOffset = topOffset != null ? topOffset : 0;

                this.displayPopupContainer(element, width, height, leftOffset, topOffset);

                //workaround for IE 6 - with divs and select (divs are appearing behind select elements)
                this.showHiddenFrame(element, width, height, leftOffset, topOffset);
                // end of workaround

                setElementsPosition(element, leftOffset, topOffset);

                // save the id, onblur event of the element that fires the popup
                // and also save the instance of this object - for $(document).click and $(window).resize
                this.triggerElementId = element.id;
                this.blur = element.onblur;
                element.onblur = null;
                popupInstance = this;
            }

            stopEventPropagation(event);
        }

        function _hide() {
            $(".modal_popup_container").hide();
            if (this.triggerElementId != null) {
                // Assign  back the blur event (using simple Javascript, not jQuery as it is  causing onblur to be fired 2 times) and reset some variables
                $('#' + this.triggerElementId)[0].onblur = this.blur;
                if ($.isFunction(this.blur)) {
                    $('#' + this.triggerElementId)[0].onblur();
                }
                else
                    if (typeof (this.blur) == 'string') {
                        var regex = new RegExp('(?=[\w.])this(?!\w)/g');
                        eval(this.blur.replace(regex, '$(\'#' + this.triggerElementId + '\')[0]'));
                    }

                // reset variables
                this.triggerElementId = null;
                this.blur = null;
                popupInstance = null;
            }
        }

        function displayPopupContainer(element, width, height, leftOffset, topOffset) {
            if (this.wnd == null) {
                this.wnd = $('<div />');
                this.wnd.addClass('modal_popup_container');
                this.wnd.css({
                    'position': 'absolute',
                    'z-index': '999',
                    'margin': '0'
                });
            }

            this.wnd.css({ 'width': width, 'height': height });
            this.wnd.html(this.html);
            this.wnd.show();

            var parentElement = $(element).parent();
            $(parentElement).append(this.wnd);
        }

        function showHiddenFrame(element, width, height, leftOffset, topOffset) {
            if (this.hiddenFrame == null) {
                this.hiddenFrame = $('<iframe />');
                this.hiddenFrame.addClass('modal_popup_container');

                this.hiddenFrame.attr({
                    //                    "src": "javascript:'<html>I am the best</html>';",
                    "src": "http://localhost:51658/Testing.aspx",
                    "scrolling": "no",
                    "frameborder": 0
                });
                this.hiddenFrame.css({
                    "position": "absolute",
                    "border": "none",
                    "display": "block",
                    "z-index": "998",
                    "margin": "0"
                });
            }

            this.hiddenFrame.css({ "width": width, "height": height });
            this.hiddenFrame.show();

            var parentElement = $(element).parent();
            $(parentElement).append(this.hiddenFrame);
        }

        function setElementsPosition(element, leftOffset, topOffset) {
            $(".modal_popup_container").position({
                my: "left top",
                at: "left bottom",
                of: $(element),
                offset: leftOffset + " " + topOffset,
                collision: "fit"
            });
        }

        function stopEventPropagation(event) {
            // manage FireFox and IE events, check if event was passed, else try and get window.event
            var e = (!event) ? window.event : event;
            // avoid that the current click event propagates up
            if (typeof (e) != "undefined") {
                if (e.stopPropagation)
                    e.stopPropagation();
                else e.cancelBubble = true;
            }
        }

        // hide the popup when user clicks outside it
        $(document).click(
     function (event) {
         if ($(event.target).closest('.modal_popup_container').get(0) == null
         && popupInstance != null) {
             popupInstance.hide();
         }
     });

        $(window).resize(
     function (event) {
         if (popupInstance != null) {
             popupInstance.hide();
         }
     });
 
    </script>
    <script language="javascript" type="text/javascript">

        var popup = createCrossBrowserPopup();
        popup.setHTML("<div style='width:100%; height:100%;background-color:#AAFFEE;border-width: 2px; border-color: silver; border-style: solid;' onclick='changePopupHTML();'> test </div>", null);

        function changePopupHTML() {
            popup.setHTML("<div style='width:100%; height:100%; background-color:#AA11EE;border-width: 2px; border-color: silver; border-style: solid;'> test222 </div>", null);
        }
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btn_Facebook" runat="server" Text="Connect To Facebook" OnClick="btn_Facebook_Click"
                    OnClientClick="popup.show(25, 0, 100, 50, this, event);" />
                <br />
                <%--<input type="submit" id="txtExample" onclick="popup.show(25, 0, 100, 50, this, event);" />--%>
                <a href="#" id="txtExample" target="_blank" onclick="popup.show(25, 0, 100, 50, this, event);">
                    Testing</a>
                <br />
                <%--popup.show(25, 0, 100, 50, this, event);--%>
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
