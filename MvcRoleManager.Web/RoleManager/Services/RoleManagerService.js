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

            service.GetRoles = function (callback) {
                $http.get('/api/rolemanager/getroles')
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
            };

            service.GetActionRoles = function (action, callback) {
                $http.post('/api/rolemanager/getactionroles', action)
                .then(
                    function (result) {
                        callback(result);
                    },
                    function () { }
                    );
            };

            service.SveActionRoles = function (action, callback) {
                 action = angular.toJson(action);

                $http.post('/api/rolemanager/SaveActionRoles',  action)
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