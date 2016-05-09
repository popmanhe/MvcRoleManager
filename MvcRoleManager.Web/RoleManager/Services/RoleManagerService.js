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

            service.login = function (user, callback) {
                $http.post('/api/Account/Login', user)
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
            };

            service.register = function (user, callback) {
                $http.post('/api/Account/Register', user)
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
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