$(document).ready(function () {
	 var actionPanelView = new app.AppView({el: "#actionPanel"});
})

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

function createConnection(callback) {
    var connection= new WebSocketManager.Connection("ws://localhost:51074/move");
    connection.connectionId = app.socketId;
    connection.enableLogging = true;
    connection.connectionMethods.onConnected = () => { }
    connection.connectionMethods.onDisconnected = () => { }
    connection.clientMethods["pingMessage"] = callback;
    connection.start();
    app.socket = connection;
}

$('input').keyup(function(e){
    if(e.keyCode == 13){
        var name = $("#playerName").val();           
        $("#playerName").remove();
        $(".name").html("Hi " + name);
        app.Name = name;
    }
  });