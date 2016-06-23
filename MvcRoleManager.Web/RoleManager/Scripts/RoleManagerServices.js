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
            Services for Users
            ************************************************/
            service.GetUsers = function (callback) {
                $http.get('/api/rolemanager/getusers')
                .then(
                function (result) {
                    callback(result.data);
                },
                function () { }
                )
            };
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

            service.GetRolesByAction = function (action, callback) {
                $http.post('/api/rolemanager/getrolesbyaction', action)
                .then(
                    function (result) {
                        callback(result.data);
                    },
                    function () { }
                    );
            };

            service.GetActionsByRole = function (role, callback) {
                $http.get('/api/rolemanager/GetActionsByRole/' + role.Id)
                .then(
                    function (result) {
                        callback(result.data);
                    },
                    function () { }
                    );
            };
            //Add actions to role
            service.AddActionsToRole = function (role) {
                role = JSON.parse(angular.toJson(role));

                $http.post('/api/rolemanager/AddActionsToRole', role)
               .then(
               function (result) {

               },
               function () { }
               )
            };

            //Add roles to action
            service.AddRolesToAction = function (action) {
                action = angular.toJson(action);

                $http.post('/api/rolemanager/AddRolesToAction', action)
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