var app = app || {};

(function () {
    app.Game = Backbone.Model.extend({
        urlRoot: 'http://localhost:51074/api/game',

        defaults: {
        },

        bindEvents() {
            this.on("change:Status", function (model) {

            })
        },

        getCustomUrl: function (method, options) {

            switch (method) {
                case 'read':
                    return 'http://localhost:51074/api/game/' + this.id;
                    break;
                case 'create':
                    return 'http://localhost:51074/api/game';
                    break;
                case 'update':
                    return 'http://localhost:51074/api/game/' + app.lastMove + '/' + app.playerId;
                    break;
                case 'delete':
                    return 'http://localhost:51074/api/game/' + this.id;
                    break;
            }
        },

        sync: function (method, model, options) {
            options || (options = {});
            options.url = this.getCustomUrl(method.toLowerCase());            
            return Backbone.sync.apply(this, arguments);
        }
    })
})();