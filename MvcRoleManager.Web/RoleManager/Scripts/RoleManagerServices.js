'use strict';
; (function () {
    var app = angular.module('RoleManager');

    app.factory('RoleManagerService', ['$http', function ($http) {
        var service = {};

        service.showJSONMessage = function (msg) {
            alert(JSON.stringify(msg));
        };;
        /************************************************
        Services for Roles & Users
        ************************************************/
        service.AddRolesToUser = function (user, callback) {
            $http.post('/api/rolemanager/AddRolesToUser', user)
            .then(
                function (result) {
                    if (callback)
                        callback(result.data);
                },
                function () { }
                );
        };

        service.AddUsersToRole = function (role, callback) {
            return $http.post('/api/rolemanager/AddUsersToRole', role);
        };

        service.GetUsersByRole = function (role, callback) {
            if (!role.Id) //Don't get users for new role
                callback(null);
            $http.post('/api/rolemanager/GetUsersByRole', role)
            .then(
            function (result) {
                if (callback)
                    callback(result.data);
            },
            function () { }
            )
        };

        service.GetRolesByUser = function (user, callback) {
            $http.get('/api/rolemanager/GetRolesByUser/' + user.Id)
            .then(
            function (result) {
                callback(result.data);
            },
            function () { }
            )
        }
        /************************************************
        Services for Roles & Actions
        ************************************************/
        service.GetRolesByAction = function (action, callback) {
            $http.post('/api/rolemanager/getrolesbyaction', action)
            .then(
                function (result) {
                    callback(result.data);
                },
                function () { }
                );
        };

        service.GetActionsByRole = function (role) {
            return $http.get('/api/rolemanager/GetActionsByRole/' + role.Id)

        };
        //Add actions to role
        service.AddActionsToRole = function (role) {
            role = JSON.parse(angular.toJson(role));

            return $http.post('/api/rolemanager/AddActionsToRole', role);
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
        /************************************************
        Private method
        ************************************************/
        var convertModelStatMessage = function (modelStatMessages) {
            var messages = [];
            for (var key in modelStatMessages) {
                modelStatMessages.forEach(function (message) {
                    messages.push(message);
                });
            }

            return messages;
        }
        return service;

    }]);
})();