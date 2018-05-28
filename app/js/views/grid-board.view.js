var app = app || {};

(function ($) {
    app.GridView = Backbone.View.extend({

        initialize: function () {           
        },

        events: {
            "click td": "clicked"
        },

        render: function () {
        },

        clicked: function (e) {
            var moveID = e.currentTarget.dataset.board;  
            app.lastMove = moveID;         
            this.addPlayersMove(moveID);
            this.notifyMove(moveID);
            $(".turn").html("");
        },

        addPlayersMove: function (moveID) {
            var player = this.getPlayer()
            // Check if player has made a move or move is valid
            if (!player.moves && player.moves.indexOf(moveID)) {
                return;
            }

            if (player && player.id == app.playerId) {  
                that = this;               
                var resp = this.model.sync("update", this.model, {
                    success: function (response) {
                        that.pushMove(player, moveID);
                        app.Game.attributes = response;
                        $('#board').find('td').prop('disabled', true);
                        that.checkMatchStatus(response.status, player.id);
                    },
                    error: function (response) {                        
                        alert("Waiting for other player to start");
                        $("#cross").remove();
                        $("#oval").remove();
                    }                    
                });
            }
        },

        pushMove: function(player, moveID) {
            if (player.moves.indexOf(moveID)) {
            player.moves.push(moveID);
            }
        },

        checkMatchStatus: function(status, playerId){
            if (status == GameStatus.Won.toLocaleString()){
                $('#board').find('td').prop('disabled', true);
                if(playerId == app.playerId){
                    var message = (app.Name ? app.Name : "You") + " have won";
                    alert(message);
                }         
                else{
                    alert("Other player has won");
                }
            }
        },

        getPlayer: function () {
            var player;
            if (this.model.attributes.player1.id == app.playerId) {
                player = this.model.attributes.player1;
            } else if (this.model.attributes.player2.id == app.playerId) {
                player = this.model.attributes.player2;
            }
            return player;
        },

        getSymbolTemplate: function (symbol) {
            app.Game
            var symboltemplate;
            if (symbol == 'X') {
                symboltemplate = _.template($("#cross").html(), {});
            } else if (symbol == 'O') {
                symboltemplate = _.template($("#oval").html(), {});
            }
            return symboltemplate;
        },

        markPlayerMove: function (playerId, moveID) {
            var symbol;
            if (this.model.attributes.player1.id == playerId) {
                symbol = this.model.attributes.player1.symbol;
            } else if (this.model.attributes.player2.id == playerId) {
                symbol = this.model.attributes.player2.symbol;
            }

            if ( $('#square' + moveID).html() == '') {
            var template = this.getSymbolTemplate(symbol);
            $('#square' + moveID).append(template);
            }
        },

        bindEvents: function () {
           
        },

        notifyMove: function (moveID) {
            app.socket.waitForConnection(function () {                
                app.socket.invoke("SendMove", app.socket.connectionId, moveID, app.playerId, app.Game.attributes.status.toLocaleString());
                if (typeof callback !== 'undefined') {
                    callback();
                }
            }, 1000);
        }
    });
})(jQuery);
