@Imports System.Web.Http
@Imports System.Web.Http.Controllers
@Imports System.Web.Http.Description
@Imports System.Collections.ObjectModel
@Imports WebSocketsServer.API.Areas.HelpPage
@ModelType Collection(Of ApiDescription)

@Code
    ViewData("Title") = "ASP.NET Web API Help Page"
    
    ' Group APIs by controller
    Dim apiGroups As ILookup(Of HttpControllerDescriptor, ApiDescription) = Model.ToLookup(Function(api) api.ActionDescriptor.ControllerDescriptor)
End Code

<link type="text/css" href="~/Areas/HelpPage/HelpPage.css" rel="stylesheet" />
<header class="help-page">
    <div class="content-wrapper">
        <div class="float-left">
            <h1>@ViewData("Title")</h1>
        </div>
    </div>
</header>
<div id="body" class="help-page">
    <section class="featured">
        <div class="content-wrapper">
            
        </div>
    </section>
    <section class="content-wrapper main-content clear-fix">
        <script type="text/javascript">
        function connectToSocket() {
            var loc = window.location, new_uri;
            if (loc.protocol === "https:") {
                new_uri = "wss:";
            } else {
                new_uri = "ws:";
            }
            new_uri += "//" + loc.host;
            new_uri += "/WebSocketsServer/WebSocketsServer.ashx";
            let host = new_uri;
            let connection = new WebSocket(host);

            connection.onopen = () => $(".btn").css("color", "green");
            connection.onmessage = (message) => {
                let data = window.JSON.parse(message.data);

                $("<li/>").html(data).appendTo($('#messages'));
            };
        }

        </script>
        <h2>Event Sample</h2>
        <input type="button" id="connect" value="Connect" class="btn" onclick="connectToSocket()" />
        <p id="Status"></p>
        <ul id="messages" />
    </section>
</div>
