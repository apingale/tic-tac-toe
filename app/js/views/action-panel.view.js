var app = app || {};

(function ($) {
    app.AppView = Backbone.View.extend({
        events: {
            "click #startGame": "createGame",
            "click #symbolButton": "pickSymbol",
            "click #restart": "reset",           
        },

        initialize: function () {
            this.bindEvents();
        },

        bindEvents: function () {
            // this.on("click #startGame", this.createGame());
        },

        render: function ($container) {
            return this;
        },

        createGame: function (e) {
            app.Game = new app.Game();
            app.Game.on("sync", function (model, response, option) {
                var gridView = new app.GridView({ el: $("#board"), model: app.Game });
                app.playerId = response.playedBy;
                app.socketId = response.socketID;
                app.symbol = response.playedBy == response.player1.id ? response.player1.symbol : response.playedBy == response.player2.id ?  response.player2.symbol : ''; 
                $('#startGame').prop('disabled', true);
                createConnection((socketID, moveId, playerId, status) => {                    
                    var symboltemplate = gridView.markPlayerMove(playerId, moveId);
                    $('#board').find('td').prop('disabled', false);
                    if(playerId != app.playerId) {
                        $(".turn").html("it's your turn"); 
                    }                    
                });
            });
            app.Game.save();
            var gridTemplate = _.template($("#grid").html(), {});
            $("#gridboard").append(gridTemplate);
        },
       
        newPlayer: function () {
            return guid();
        }
    });
})(jQuery);

