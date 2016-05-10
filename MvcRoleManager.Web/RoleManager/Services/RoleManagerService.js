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
                $http.post('/api/rolemanager/SaveActionPermissions', controllers)
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