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

            service.GetControllers = function (callback) {
                $http.get('/api/RoleManager/GetControllers').then(
                    function (result) {//success
                        callback(result.data);
                    }
                , function () {//failed

                });
            }

            service.GetGroups = function (callback) {
                //$http.get('/api/RoleManager/GetGroups').then(
                //    function (result) {//success
                callback([
                    { 'Name': 'group2', 'Description': 'group2 desc', 'stat':'view' }
                    , { 'Name': 'group1', 'Description': 'group1 desc', 'stat':'view' }
                ]);
                //    }
                //, function () {//failed

                //});
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

            service.SveActionRoles = function (action) {
                action = angular.toJson(action);

                $http.post('/api/rolemanager/SaveActionRoles', action)
               .then(
               function (result) {

               },
               function () { }
               )
            };

            return service;

        }]);
    }]);

})();