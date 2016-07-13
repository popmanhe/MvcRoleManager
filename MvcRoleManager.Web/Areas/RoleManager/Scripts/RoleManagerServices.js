'use strict';
(function () {
    var app = angular.module('RoleManager');

    app.factory('RoleManagerService', ['$http', function ($http) {
        var service = {};

        service.showJSONMessage = function (msg) {
            alert(JSON.stringify(msg));
        };

        /************************************************
        Services for Roles & Users
        ************************************************/

        service.AddRolesToUser = function (user) {
            return $http.post('/api/rolemanager/AddRolesToUser', user);
        };

        service.AddUsersToRole = function (role) {
            return $http.post('/api/rolemanager/AddUsersToRole', role);
        };

        service.GetUsersByRole = function (role) {
            if (!role.Id) //Don't get users for new role
                return;
            return $http.post('/api/rolemanager/GetUsersByRole', role);
            
        };

        service.GetRolesByUser = function (user) {
            return $http.get('/api/rolemanager/GetRolesByUser/' + user.Id);
        };

        /************************************************
        Services for Roles & Actions
        ************************************************/

        service.GetRolesByAction = function (action) {
            return $http.post('/api/rolemanager/getrolesbyaction', action);
        };

        service.GetActionsByRole = function (role) {
            return $http.get('/api/rolemanager/GetActionsByRole/' + role.Id);
        };
        //Add actions to role
        service.AddActionsToRole = function (role) {
            role = JSON.parse(angular.toJson(role));

            return $http.post('/api/rolemanager/AddActionsToRole', role);
        };

        //Add roles to action
        service.AddRolesToAction = function (action) {
            action = angular.toJson(action);

            return $http.post('/api/rolemanager/AddRolesToAction', action);
             
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
        };
        return service;

    }]);
})();