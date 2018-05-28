var app = app || {};

(function () { 
    app.ToDo = Backbone.Model.extend({
       
        defaults: {           
            name: '',           
            status: GameStatus.New,
            moves:[],
            symbol: ''
        },
    })
})();