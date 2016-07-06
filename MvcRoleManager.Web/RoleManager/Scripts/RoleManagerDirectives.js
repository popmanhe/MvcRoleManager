﻿'use strict';
; (function () {
    var app = angular.module('RoleManager');
    //roles directive
    app.controller('MvcRoleCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {
        $scope.Roles = [];
        $scope.selectedRole;

        //Directive methods
        RoleManagerService.GetRoles(function (data) {
            if (data) {
                data.forEach(function (g) { g.stat = 'view'; });
                $scope.Roles = data;
                if (data && data.length > 0) {
                    $scope.ItemClick(data[0]);
                }

            }
        });

        $scope.SetItemClass = function (role) {
            if ($scope.selectedRole === role) {
                return "active";
            } else {
                return "";
            }
        }

        $scope.AddRole = function () {
            var role = {
                Name: '',
                stat: 'new'
            };
            $scope.Roles.unshift(role);
            $scope.ItemClick(role);
            $scope.adding = true;
        }

        $scope.EditRole = function (role) {
            role.stat = 'edit';
        }

        $scope.UpdateRole = function (role) {//Update role's name only
            if (role.stat === 'new') {
                RoleManagerService.AddRole(role, function (data) {
                    role.Id = data;
                    role.stat = 'view';
                    $scope.adding = false;
                });
            }
            else {
                role.Users = null;
                RoleManagerService.UpdateRole(role, function () {
                    role.stat = 'view';
                    $scope.adding = false;
                });
            }
        }

        $scope.DeleteRole = function (role) {
            if (confirm("Are you sure to delete role," + role.Name + "?")) {
                RoleManagerService.DeleteRole(role, function () {
                    $scope.Roles = $scope.Roles.filter(function (g) {
                        return g.Name !== role.Name;
                    });

                    if ($scope.Roles.length > 0)
                        $scope.ItemClick($scope.Roles[0]);
                    $scope.OnItemDelete({ role: $scope.selectedRole });
                });
            }
        }

        $scope.CancelUpdate = function (role) {
            if (role.stat === 'new') {
                $scope.Roles.shift(role);
                $scope.adding = false;
            }
            else { role.stat = 'view'; }
        }


        //public attributes
        $scope.ItemClick = function (role) {
            $scope.selectedRole = role;
            $scope.onItemclick({ role: $scope.selectedRole });
        }

        $scope.Save = function () {
            $scope.onSave({ role: $scope.selectedRole });
        }

        $scope.Cancel = function () {
            $scope.onCancel({ role: $scope.selectedRole });
        }
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
                Properties: '=properties'
            },
            controller: 'MvcRoleCtrl',
            templateUrl: 'partials/Roles.html'
        };
    });
    //simple role directive
    app.controller('MvcSimpleRoleCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {
        var self = this;
        //Directive methods
        RoleManagerService.GetRoles(function (data) {
            if (data) {
                data.forEach(function (g) { g.stat = 'view'; });
                $scope.Properties.Roles = data;
            }
        });

        this.GetSelectedRoles = function () {
            return $scope.Properties.Roles.filter(function (role) {
                return role.Selected;
            });
        }

        $scope.SelectAll = function (select) {
            $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                role.Selected = select;
                return role;
            });
        }

        //public events
        $scope.Save = function () {
            $scope.onSave({ roles: self.GetSelectedRoles() });
        }

        $scope.Cancel = function () {
            $scope.onCancel();
        }
    }])
    .directive('mvcSimpleroles', function () {
        return {
            restrict: "E",
            scope: {
                //Events
                onSave: '&',
                onCancel: '&',
                //Properties
                Properties: '=properties'
            },
            controller: 'MvcSimpleRoleCtrl',
            templateUrl: 'partials/SimpleRole.html',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Roles = [];
                scope.Properties.AssignedTo = '';
            }
        };
    });

    //controllers directive
    app.controller('MvcControllersCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {

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
        }
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
        }
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
        }

        $scope.GetControllers = function () {
            RoleManagerService.GetControllers(function (data) {
                $scope.Properties.Controllers = data;
                $scope.Properties.Controllers.forEach(function (ctrl) {
                    ctrl.status = { 'open': true } // open Accordion header
                    ;
                })
                //select first action of first controller 
                if ( $scope.Properties.SelectFirstItem && $scope.Properties.Controllers.length > 0) {
                    $scope.selectedController = $scope.Properties.Controllers[0];

                    if ($scope.Properties.Controllers[0].Actions.length > 0) {
                        $scope.selectedAction = $scope.selectedController.Actions[0];

                        $scope.ItemClick($scope.selectedController, $scope.selectedAction);
                    }
                }
            })
        };
        $scope.GetControllers();

        $scope.SelectAll = function ($event, ctrl) {
            if (ctrl.Actions && ctrl.Actions.length > 0) {
                ctrl.Actions.forEach(function (action) {
                    action.Selected = $event.currentTarget.checked;
                })
            }
        };

        $scope.ItemClick = function (ctrl, action) {
            $scope.selectedController = ctrl;
            action.ControllerName = ctrl.ControllerName;
            $scope.selectedAction = action;
            $scope.onItemclick({ action: $scope.selectedAction });
        }

        //expose public methods at last
        $scope.Methods = {
            SetSelectedActions: $scope.SetSelectedActions
        };

        //$scope.Properties = {
        //    Controllers: $scope.Controllers
        //};
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
            templateUrl: 'partials/Controllers.html',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Controllers = [];
            }

        };
    });

    //users directive
    app.controller('MvcUserCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {
        $scope.selectedUser;
        $scope.filters = {
            Selected: null,
            selectedOnly: false,
            search: ''
        };

        //Directive methods
        RoleManagerService.GetUsers(function (data) {
            if (data) {
                data.forEach(function (u) { u.stat = 'view'; });
                $scope.Properties.Users = data;
                if ($scope.Properties.SelectFirstItem && data && data.length > 0) {
                    $scope.selectedUser = data[0];
                    $scope.ItemClick($scope.selectedUser);
                }

            }
        });

        $scope.SetItemClass = function (user) {
            if ($scope.selectedUser === user) {
                return "active";
            } else {
                return "";
            }
        }

        $scope.AddUser = function () {
            var user = {
                Email: '',
                Name: '',
                Password: '',
                ConfirmPassword: '',
                stat: 'new'
            };
            $scope.Properties.Users.unshift(user);
            $scope.adding = true;
        }

        $scope.EditUser = function (user) {
            user.Password = 'NotChanged';//provide a dummy password for UI
            user.ConfirmPassword = 'NotChanged';
            user.stat = 'edit';
        }

        $scope.UpdateUser = function (user) {//Update user's email, name, password only
            if (user.stat === 'new') {
                RoleManagerService.AddUser(user,
                    function (data) {
                        user.Id = data;
                        user.stat = 'view';
                        $scope.adding = false;
                    },
                function (error) {

                });
            }
            else {
                if (user.Password === 'NotChanged') user.Password = '';
                RoleManagerService.UpdateUser(user, function () {
                    user.stat = 'view';
                    $scope.adding = false;
                });
            }
        }

        $scope.DeleteUser = function (user) {
            if (confirm("Are you sure to delete user," + user.Name + "?")) {
                RoleManagerService.DeleteUser(user, function () {
                    $scope.Properties.Users = $scope.Properties.Users.filter(function (g) {
                        return g.Name !== user.Name;
                    });
                });
            }
        }

        $scope.CancelUpdate = function (user) {
            if (user.stat === 'new') {
                $scope.Properties.Users.shift(user);
                $scope.adding = false;
            }
            else { user.stat = 'view'; }
        }

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

        }


        //private methods
        var showMessages = function (messages) {
            $scope.Messages = messages;
        }

        //public methods
        $scope.Methods = {
            SetSelectedUsers: $scope.SetSelectedUsers
        };
        //public attributes
        $scope.ItemClick = function (user) {
            $scope.selectedUser = user;
            $scope.onItemclick({ user: $scope.selectedUser });
        }

        $scope.Save = function () {
            $scope.onSave({ user: $scope.selectedUser });
        }

        $scope.Cancel = function () {
            $scope.onCancel({ user: $scope.selectedUser });
        }
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
            templateUrl: 'partials/Users.html',
            link: function (scope, element, attrs) {
                scope.Properties = scope.Properties || {};
                scope.Properties.Users = [];
            }
        };
    });

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
