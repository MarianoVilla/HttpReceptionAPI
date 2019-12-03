
<script type="text/javascript">
    //TODO: Clean up.
    var connection;
    var interval;
    function getSocketURL() {
        var loc = window.location, uri;
        uri = loc.protocol === "https:" ? "wss:" : "ws:"
        uri += "//" + loc.host;
        uri += "/WebSocketsServer/WebSocketsServerSample.ashx";
        return uri;
    }
    function getSocketConnection() {
        let host = getSocketURL();
        console.log(host);
        connection = new WebSocket(host);
        connection.host = host;
        return connection;
    }
    function connectionTest() {

        connection = getSocketConnection();
        connection.onopen = function () {
            interval = setInterval(() => connection.send("Hey, this is a client message!"), 3000);
            $("#connect").css("color", "green");
            $("#disconnect").css("color", "red");

        }
        connection.onmessage = (message) => {
            let data = window.JSON.parse(message.data);

            $("<li/>").html(data).appendTo($('#messages'));
        };
        connection.onclose = () => {
            $("<li/>").html("We've lost connection!").appendTo($('#messages'));
            clearInterval(interval);
            $(".btn").css("color", "grey");
        }
    }
    function disconnectFromSocket() {
        if (connection) {
            connection.close();
        }
    }
    this.send = function (message, callback) {
        this.waitForConnection(function () {
            connection.send(message);
            if (typeof callback !== 'undefined') {
                callback();
            }
        }, 1000);
    };

    this.waitForConnection = function (callback, interval) {
        if (connection.readyState === 1) {
            callback();
        } else {
            var that = this;
            setTimeout(function () {
                that.waitForConnection(callback, interval);
            }, interval);
        }
    };
    function sendFile() {

        connection = connection || getSocketConnection();

        if (!connection.readyState) {
            connection = getSocketConnection();
        }
        var file = document.getElementById('filename').files[0];

        var reader = new FileReader();

        reader.loadend = function (e) {

        };

        reader.onload = function (e) {

            var rawData = e.target.result;
            var byteArray = new Uint8Array(rawData);

            globalThis.send(byteArray.buffer);

            console.log("the File has been transferred.\n");

        };

        reader.readAsArrayBuffer(file);

    }
</script>
@*TODO: Beautify.*@
<h2>Press the buttons for a simple socket connection sample!</h2>
<input type="button" id="connect" value="Connect" class="btn" onclick="connectionTest()" />
<input type="button" id="disconnect" value="Disconnect" class="btn" onclick="disconnectFromSocket()" />
<h2>File Upload</h2>   Select file
<input type="file" id="filename" />
<input type="button" value="Upload" onclick="sendFile()" />
<h2>To use the standard HTTP API, use ThisWebsite/api</h2>
<p id="Status"></p>
<ul id="messages" />