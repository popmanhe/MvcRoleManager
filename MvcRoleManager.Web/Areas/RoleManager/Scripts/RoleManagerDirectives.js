﻿'use strict';
(function () {
    var app = angular.module('RoleManager');
    var basePath = virtualPath + '/RoleManager/Home/';
    var apiPath = virtualPath + '/api/RoleManager/';
    //roles directive
    app.factory('MvcRoleService', ['$http', function ($http) {
        var service = {};

        service.GetRoles = function () {
            return $http.get(apiPath + 'getroles');
        };

        service.AddRole = function (role) {
            return $http.post(apiPath + 'AddRole', role);
        };
        service.UpdateRole = function (role, callback) {
            return $http.post(apiPath + 'UpdateRole', role);
        };

        service.DeleteRole = function (role, callback) {
            return $http.post(apiPath + 'DeleteRole', role);
        };

        return service;
    }])
    .controller('MvcRoleCtrl', ['$scope', 'MvcRoleService', function ($scope, MvcRoleService) {
        var self = this;
        $scope.Roles = [];
        $scope.selectedRole;
        $scope.Message = { Content: '' };

        $scope.Methods = {
            ClearMessage: function () {
                $scope.Message.Content = '';
            },
            SetMessage: function (message) {
                $scope.Message = message;
            }
        };
        //Directive methods
        MvcRoleService.GetRoles()
        .then(
            function (result) {
                var data = result.data;
                if (data) {
                    data.forEach(function (g) { g.stat = 'view'; });
                    $scope.Roles = data;
                    if (data && data.length > 0) {
                        $scope.ItemClick(data[0]);
                    }
                };
            },
            function () { }
      );

        $scope.SetItemClass = function (role) {
            if ($scope.selectedRole === role) {
                return "active";
            } else {
                return "";
            }
        };

        $scope.AddRole = function () {
            var role = {
                Name: '',
                stat: 'new'
            };
            $scope.Roles.unshift(role);
            $scope.ItemClick(role);
            $scope.adding = true;
        };

        $scope.EditRole = function (role) {
            role.stat = 'edit';
        };

        $scope.UpdateRole = function (role) {//Update role's name only
            if (role.stat === 'new') {
                MvcRoleService.AddRole(role)
                 .then(
                    function (result) {
                        var data = result.data;
                        role.Id = data;
                        role.stat = 'view';
                        $scope.adding = false;
                        $scope.Message = { Content: 'Role added', Type: 'success' };
                    },
                    function (result) {
                        if (result.data.ExceptionMessage)
                            $scope.Message = { Content: 'Add role failed', Type: 'danger' };
                    }
                    );
            }
            else {
                role.Users = null;
                MvcRoleService.UpdateRole(role)
                .then(
                    function () {
                        role.stat = 'view';
                        $scope.adding = false;
                        $scope.Message = { Content: 'Role added', Type: 'success' };
                    },
                    function (result) {
                        if (result.data.ExceptionMessage)
                            $scope.Message = { Content: 'Update role failed', Type: 'danger' };
                    });
            }
        };

        $scope.DeleteRole = function (role) {
            if (confirm("Are you sure to delete role," + role.Name + "?")) {
                MvcRoleService.DeleteRole(role)
                .then(
                    function () {
                        $scope.Roles = $scope.Roles.filter(function (g) {
                            return g.Name !== role.Name;
                        });

                        if ($scope.Roles.length > 0)
                            $scope.ItemClick($scope.Roles[0]);
                        $scope.OnItemDelete({
                            role: $scope.selectedRole
                        });
                    },
                    function () { }
                    );
            }
        };

        $scope.CancelUpdate = function (role) {
            if (role.stat === 'new') {
                $scope.Roles.shift(role);
                $scope.adding = false;
            }
            else { role.stat = 'view'; }
        };


        //public events
        $scope.ItemClick = function (role) {
            $scope.Methods.ClearMessage();
            $scope.selectedRole = role;
            $scope.onItemclick({ role: $scope.selectedRole });
        };

        $scope.Save = function () {
            $scope.Methods.ClearMessage();
            $scope.onSave({
                role: $scope.selectedRole
            });
        };

        $scope.Cancel = function () {
            $scope.Methods.ClearMessage();
            $scope.onCancel({ role: $scope.selectedRole });
        };
    }])
    .directive('mvcRoles', function () {
        return {
            restrict: "E",
            scope: {
                //Events
                OnItemDelete: '&',
                onItemclick: '&',
                onSave: '&',
                onCancel: '&',
                //Properties
                Properties: '=properties',
                Methods: '=methods'
            },
            controller: 'MvcRoleCtrl',
            templateUrl: basePath + 'Roles'
        };
    });

    //simple role directive. share the same service as mvcRoles directive
    app.controller('MvcSimpleRoleCtrl', ['$scope', 'MvcRoleService', function ($scope, MvcRoleService) {
        var self = this;
        $scope.Message = { Content: '' };

        $scope.Methods = {
            ClearMessage: function () {
                $scope.Message.Content = '';
            },
            SetMessage: function (message) {
                $scope.Message = message;
            }
        };
        //Directive methods
        MvcRoleService.GetRoles().then(
            function (result) {
                var data = result.data;
                if (data) {
                    data.forEach(function (g) { g.stat = 'view'; });
                    $scope.Properties.Roles = data;
                }
            },
            function () { }
            );

        this.GetSelectedRoles = function () {
            return $scope.Properties.Roles.filter(function (role) {
                return role.Selected;
            });
        };

        $scope.SelectAll = function (select) {
            $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                role.Selected = select;
                return role;
            });
        };

        //public events
        $scope.Save = function () {
            $scope.Methods.ClearMessage();
            $scope.onSave({ roles: self.GetSelectedRoles() });
        };

        $scope.Cancel = function () {
            $scope.Methods.ClearMessage();
            $scope.onCancel();
        };
    }])
    .directive('mvcSimpleroles', function () {
        return {
            restrict: "E",
            scope: {
                //Events
                onSave: '&',
                onCancel: '&',
                //Properties
                Properties: '=properties',
                //Methods
                Methods: '=methods'
            },
            controller: 'MvcSimpleRoleCtrl',
            templateUrl: basePath + 'SimpleRole',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Roles = [];
                scope.Properties.AssignedTo = '';
                scope.Methods = scope.Methods || {};
            }
        };
    });

    //controllers directive
    app.factory('MvcControllersService', ['$http', function ($http) {
        /************************************************
           Services for controllers and actions from assembly
        ************************************************/
        var service = {};
        service.GetControllers = function (callback) {
            return $http.get(apiPath + 'GetControllers');
        };
        return service;
    }])
    .controller('MvcControllersCtrl', ['$scope', 'MvcControllersService', function ($scope, MvcControllersService) {

        $scope.selectedController;
        $scope.selectedAction;
        $scope.filters = {
            selectedOnly: false,
            search: ''
        };
        $scope.SetItemClass = function (action) {
            if ($scope.selectedAction === action) {
                return "info";
            } else {
                return "";
            }
        };
        this.originControllers = [];
        $scope.showSelectedOnly = function () {
            if ($scope.filters.selectedOnly) {
                this.originControllers = angular.copy($scope.Properties.Controllers);
                $scope.Properties.Controllers = $scope.Properties.Controllers.filter(function (ctrl) {
                    ctrl.Actions = ctrl.Actions.filter(function (act) {
                        return act.Selected;
                    });
                    return ctrl.Actions && ctrl.Actions.length > 0;
                });
            }
            else {
                $scope.Properties.Controllers = angular.copy(this.originControllers);
            }
        };
        $scope.SetSelectedActions = function (selectedActions) {
            if (selectedActions) {
                $scope.Properties.Controllers.forEach(function (ctrl) {
                    ctrl.Actions.forEach(function (action) {
                        action.Selected = false;
                        selectedActions.forEach(function (selectedAction) {
                            if (ctrl.ControllerName === selectedAction.ControllerName &&
                                 action.ActionName === selectedAction.ActionName &&
                                 action.ReturnType === selectedAction.ReturnType &&
                                 action.ParameterTypes.join() === selectedAction.ParameterTypes) {
                                action.Selected = true;
                            }
                        });
                    });
                });
            }
        };

        $scope.GetControllers = function () {
            MvcControllersService.GetControllers()
            .then(
                function (result) {
                    var data = result.data;
                    $scope.Properties.Controllers = data;
                    $scope.Properties.Controllers.forEach(function (ctrl) {
                        ctrl.status = { 'open': true } // open Accordion header
                        ;
                    });
                    //select first action of first controller 
                    if ($scope.Properties.SelectFirstItem && $scope.Properties.Controllers.length > 0) {
                        $scope.selectedController = $scope.Properties.Controllers[0];

                        if ($scope.Properties.Controllers[0].Actions.length > 0) {
                            $scope.selectedAction = $scope.selectedController.Actions[0];

                            $scope.ItemClick($scope.selectedController, $scope.selectedAction);
                        }
                    };
                },
                function () { }
            );
        };
        $scope.GetControllers();

        $scope.SelectAll = function ($event, ctrl) {
            if (ctrl.Actions && ctrl.Actions.length > 0) {
                ctrl.Actions.forEach(function (action) {
                    action.Selected = $event.currentTarget.checked;
                });
            }
        };

        $scope.ItemClick = function (ctrl, action) {
            $scope.selectedController = ctrl;
            action.ControllerName = ctrl.ControllerName;
            $scope.selectedAction = action;
            $scope.onItemclick({ action: $scope.selectedAction });
        };

        //expose public methods at last
        $scope.Methods = {
            SetSelectedActions: $scope.SetSelectedActions
        };
    }])
    .directive('mvcControllers', function () {
        return {
            restrict: "E",
            scope: {
                //public events
                onItemclick: '&',
                //public methods
                Methods: '=methods',
                //public property
                Properties: '=properties'
            },
            controller: 'MvcControllersCtrl',
            templateUrl: basePath + 'Controllers',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Controllers = [];
            }

        };
    });

    //users directive
    app.factory('MvcUserService', ['$http', function ($http) {
        var service = {};
        service.GetUsers = function (callback) {
            return $http.get(apiPath + 'getusers');

        };

        service.AddUser = function (user) {
            return $http.post(apiPath + 'AddUser', user);

        };

        service.UpdateUser = function (user, callback) {
            return $http.post(apiPath + 'UpdateUser', user);
        };

        service.DeleteUser = function (user, callback) {
            return $http.post(apiPath + 'DeleteUser', user);
        };
        service.Login = function (user) {
            var loginData = 'grant_type=password&username=' + user.UserName + '&password=' + user.Password;
            return $http({
                method: 'POST',
                url: '/Token',
                data: loginData,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        };

        return service;
    }])
    .controller('MvcUserCtrl', ['$scope', 'MvcUserService', '$uibModal', function ($scope, MvcUserService, $uibModal) {
        var self = this;
        $scope.selectedUser;
        $scope.filters = {
            Selected: null,
            selectedOnly: false,
            search: ''
        };

        //Directive methods
        MvcUserService.GetUsers()
        .then(function (result) {
            var data = result.data;
            if (data) {
                data.forEach(function (u) { u.stat = 'view'; });
                $scope.Properties.Users = data;
                if ($scope.Properties.SelectFirstItem && data && data.length > 0) {
                    $scope.selectedUser = data[0];
                    $scope.ItemClick($scope.selectedUser);
                }
            }
        }
        , function (result) { });

        $scope.SetItemClass = function (user) {
            if ($scope.selectedUser === user) {
                return "active";
            } else {
                return "";
            }
        };
        var tokenKey = 'accessToken';
        $scope.Login = function (user) {

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'userPassword.html',
                controller: 'UserPasswordModalCtrl',
                size: 'sm'
            });

            modalInstance.result.then(function (password) {
                if (password !== '') {
                    user.Password = password;
                    MvcUserService.Login(user)
               .then(
               function (result) {
                   sessionStorage.setItem(tokenKey, result.data.access_token);
               },
               function (result) {

               });
                }
            });
        };

        $scope.AddUser = function () {
            var user = {
                Email: '',
                Name: '',
                Password: '',
                ConfirmPassword: '',
                stat: 'new',
                Message: { Content: '', Type: '' }
            };
            $scope.Properties.Users.unshift(user);
            $scope.selectedUser = user;
            $scope.adding = true;
        };

        $scope.EditUser = function (user) {
            user.Password = 'NotChanged';//provide a dummy password for UI
            user.ConfirmPassword = 'NotChanged';
            user.stat = 'edit';
        };

        $scope.UpdateUser = function (user) {//Update user's email, name, password only
            if (user.stat === 'new') {
                MvcUserService.AddUser(user)
                    .then(
                        function (result) {
                            user.Id = result.data;
                            user.stat = 'view';
                            $scope.adding = false;
                            showMessages({ Content: 'User added.', Type: 'success' });
                        },
                        function (result) {
                            if (result.data.ExceptionMessage)
                                showMessages({ Content: result.data.ExceptionMessage, Type: 'danger' });
                        }
                        );
            }
            else {
                if (user.Password === 'NotChanged') user.Password = '';
                MvcUserService.UpdateUser(user)
                .then(
                    function (result) {
                        user.stat = 'view';
                        $scope.adding = false;
                        showMessages({ Content: 'User updated.', Type: 'success' });
                    },
                    function (result) {
                        if (result.data.ExceptionMessage)
                            showMessages({ Content: result.data.ExceptionMessage, Type: 'danger' });
                    }
                );
            }
        };

        $scope.DeleteUser = function (user) {
            if (confirm("Are you sure to delete user," + user.Name + "?")) {
                MvcUserService.DeleteUser(user)
                .then(
                function (result) {
                    $scope.Properties.Users = $scope.Properties.Users.filter(function (u) {
                        return u.Id !== user.Id;
                    });
                },
                function () { }
                );
            }
        };

        $scope.CancelUpdate = function (user) {
            if (user.stat === 'new') {
                $scope.Properties.Users.shift(user);
                $scope.adding = false;
            }
            else { user.stat = 'view'; }
        };

        $scope.SetSelectedUsers = function (selectedUsers) {
            $scope.Properties.Users.forEach(function (user) {
                user.Selected = false;
                if (selectedUsers)
                    selectedUsers.forEach(function (selectedUser) {
                        if (user.Id === selectedUser) {
                            user.Selected = true;
                        }
                    });
            });
        };


        //private methods
        var showMessages = function (messages) {
            $scope.selectedUser.Message = messages;
        },
            clearMessage = function () {
                $scope.selectedUser.Message = '';
            };

        //public methods
        $scope.Methods = {
            SetSelectedUsers: $scope.SetSelectedUsers
        };
        //public attributes
        $scope.ItemClick = function (user) {
            if ($scope.selectedUser != user)
                clearMessage();
            $scope.selectedUser = user;
            $scope.onItemclick({ user: $scope.selectedUser });
        };

        $scope.Save = function () {
            clearMessage();
            $scope.onSave({ user: $scope.selectedUser });
        };

        $scope.Cancel = function () {
            clearMessage();
            $scope.onCancel({ user: $scope.selectedUser });
        };
    }])
    .directive('mvcUsers', function () {
        return {
            restrict: "E",
            scope: {
                showfooter: '@',
                onItemclick: '&',
                onSave: '&',
                onCancel: '&',
                Methods: '=methods',
                Properties: '=properties'
            },
            controller: 'MvcUserCtrl',
            templateUrl: basePath + 'Users',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Users = [];
            }
        };
    })
.controller('UserPasswordModalCtrl', ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {

    $scope.Password = '';
    $scope.ok = function () {
        $uibModalInstance.close($scope.Password);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);

    //messages directive
    app.controller('MessageCtrl', ['$scope', '$attrs', '$interpolate', '$timeout', function ($scope, $attrs, $interpolate, $timeout) {
        $scope.closeable = !!$attrs.close;
        $scope.Messages = [];
        var dismissOnTimeout = angular.isDefined($attrs.dismissOnTimeout) ?
          $interpolate($attrs.dismissOnTimeout)($scope.$parent) : null;

        if (dismissOnTimeout) {
            $timeout(function () {
                $scope.close();
            }, parseInt(dismissOnTimeout, 10));
        }
    }])
    .directive('mvcMessages', function () {
        return {
            controller: 'MessageCtrl',
            scope: {
                Messages: '=messages',
                type: '@',
                close: '&'
            },
            template: '<div class="alert ng-scope ng-isolate-scope alert-success alert-dismissible" ng-class="[\'alert-\' + (type || \'warning\'), closeable ? \'alert-dismissible\' : null]" role="alert" close="closeAlert($index)"><button ng-show="closeable" type="button" class="close" ng-click="close({$event: $event})"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button><ul><li ng-repeat="alert in alerts" ></li>/div>'
        };
    });
})();
