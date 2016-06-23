'use strict';
; (function () {
    var app = angular.module('RoleManager');
    //roles directive
    app.controller('RoleCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {
        $scope.Roles = [];
        $scope.selectedRole;

        //Directive methods
        RoleManagerService.GetRoles(function (data) {
            if (data) {
                data.forEach(function (g) { g.stat = 'view'; });
                $scope.Roles = data;
                if (data && data.length > 0)
                    $scope.selectedRole = data[0];
            }
        });

        $scope.SetItemClass = function (role) {
            if ($scope.selectedRole == role) {
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
            $scope.adding = true;
        }

        $scope.EditRole = function (role) {
            role.stat = 'edit';
        }

        $scope.UpdateRole = function (role) {//Update role's name only
            if (role.stat == 'new') {
                RoleManagerService.AddRole(role, function (data) {
                    role.Id = data.Id;
                    role.stat = 'view';
                    $scope.adding = false;
                });
            }
            else {
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
                        return g.Name != role.Name;
                    });
                });
            }
        }

        $scope.CancelUpdate = function (role) {
            if (role.stat == 'new') {
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
    .directive('roles', function () {
        return {
            restrict: "E",
            scope: {
                onItemclick: '&',
                onSave: '&',
                onCancel: '&'
            },
            controller: 'RoleCtrl',
            templateUrl: 'partials/Roles.html',
            link: function (scope, element, attrs) {
                var s = scope;
            }
        };
    });

    //controllers directive
    app.controller('ControllersCtrl', ['$scope', 'RoleManagerService', function ($scope, RoleManagerService) {

        $scope.Controllers = [];
        $scope.selectedController;
        $scope.selectedAction;

        $scope.SetItemClass = function (action) {
            if ($scope.selectedAction == action) {
                return "info";
            } else {
                return "";
            }
        }


        $scope.GetControllers = function () {
            RoleManagerService.GetControllers(function (data) {
                $scope.Controllers = data;
                //select first action of first controller 
                if ($scope.Controllers.length > 0) {
                    $scope.selectedController = $scope.Controllers[0];

                    if ($scope.Controllers[0].Actions.length > 0)
                        $scope.selectedAction = $scope.selectedController.Actions[0];


                    $scope.selectedAction.ControllerName = $scope.selectedController.ControllerName;
                    $scope.onItemclick({ action: $scope.selectedAction });
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
            $scope.onItemclick({action: $scope.selectedAction });
        }

    }])
    .directive('controllers', function () {
        return {
            restrict: "E",
            scope: {
                onItemclick: '&',
                showcheckbox: '@',
                control: '='
            },
            controller: 'ControllersCtrl',
            templateUrl: 'partials/Controllers.html',
            link: function (scope, element, attrs) {
                scope.internalControl = scope.control || {};
                scope.internalControl.GetActionsByRole = scope.GetActionsByRole;
            }
        };
    });

})();