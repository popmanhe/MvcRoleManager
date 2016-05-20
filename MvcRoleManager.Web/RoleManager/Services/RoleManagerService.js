'use strict';
; (function () {
    var app = angular.module('RoleManager');

    app.config(['$provide', function ($provide) {
        $provide.factory('RoleManagerService', ['$http', function ($http) {

            var service = {};

            var showJSONMessage = function (msg) {
                alert(JSON.stringify(msg));
            };

            service.showJSONMessage = showJSONMessage;

            service.getControllers = function (callback) {
                $http.get('/api/RoleManager/GetControllers').then(
                    function (result) {//success
                        callback(result.data);
                  }
                , function () {//failed

                });
            }
          
            service.getRoles = function (callback) {
                $http.get('/api/rolemanager/getroles')
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
            };

            service.getActionRoles = function(action)
            {
                $http.get('/api/rolemanager/getactionroles').then(
                    function (result) {

                    },
                    function(){}
                    );
            };

            service.saveActionPermissions = function (controllers, callback) {
                var actions = [];
                controllers.forEach(function (controller) {
                    if (controller.Actions)
                    {
                        controller.Actions.forEach(function (action) {
                            if (action.modified)
                            {
                                actions.push(action);
                            }
                        });
                    }
                });
                $http.post('/api/rolemanager/SaveActionRoles', angular.toJson(actions))
               .then(
               function (result) {
                   callback(result);
               },
               function () { }
               )
            };

            return service;

        }]);
    }]);

})();