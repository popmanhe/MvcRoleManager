'use strict';
; (function () {
    var app = angular.module('RoleManager');

    app.config(['$provide', function ($provide) {
        $provide.factory('RoleManagerService', ['$http', function ($http) {

            var service = {};

            service.showJSONMessage = function (msg) {
                alert(JSON.stringify(msg));
            };;
            /************************************************
            Services for controllers and actions from assembly
            ************************************************/
            service.GetControllers = function (callback) {
                $http.get('/api/RoleManager/GetControllers').then(
                    function (result) {//success
                        callback(result.data);
                    }
                , function () {//failed

                });
            }
            /************************************************
            Services for Roles
            ************************************************/
            service.GetRoles = function (callback) {
                $http.get('/api/rolemanager/getroles')
                .then(
                function (result) {
                    callback(result.data);
                },
                function () { }
                )
            };

            service.AddRole = function (role, callback) {
                $http.post('/api/RoleManager/AddRole', role).then(
                   function (result) {//success
                       callback(result.data);
                   }
               , function (result) {//failed
                   alert(result.data.ExceptionMessage);
               });
            }
            service.UpdateRole = function (role, callback) {
                $http.post('/api/RoleManager/UpdateRole', role).then(
                   function (result) {//success
                       callback();
                   }
               , function () {//failed

               });
            }

            service.DeleteRole = function (role, callback) {
                $http.post('/api/RoleManager/DeleteRole', role).then(
                   function (result) {//success
                       callback();
                   }
               , function () {//failed

               });
            }
            /************************************************
            Services for roles
            ************************************************/
            

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