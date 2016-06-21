'use strict';
; (function () {
    var app = angular.module('RoleManager');
    var basePath = '/RoleManager/partials/';
    //Config routes.
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/");

        // Now set up the states
        $stateProvider
          .state('controllers', {
              url: "/",
              templateUrl: basePath + "Controllers.html",
              controller: 'ControllersCtrl'
          })
        .state('actionrole', {
            url: "/actionrole",
            templateUrl: basePath + "ActionRole.html",
            controller: 'ActionRoleCtrl'
        });
    }]);

    app.controller('TabsCtrl', ['$scope', function ($scope) {
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
            $scope.tabs.forEach(function (tab) {
                if (tab.link == '#/' + toState.name)
                    $scope.selectedTab = tab;
            });
        });

        $scope.tabs = [
            { link: '#/', label: 'Roles->Action' },
            { link: '#/actionrole', label: 'Actions -> Role' }
        ];
        $scope.selectedTab = $scope.tabs[0];
        $scope.setSelectedTab = function (tab) {
            $scope.selectedTab = tab;
        }

        $scope.tabClass = function (tab) {
            if ($scope.selectedTab == tab) {
                return "active";
            } else {
                return "";
            }
        }
    }]);
    
    //Assign actions to role
    app.controller('ActionRoleCtrl', ['$scope', 'RoleManagerService',
       function ($scope, RoleManagerService) {
           $scope.selectedRole;
           $scope.Controllers = [];
           $scope.Roles = [];
           $scope.adding = false;
           RoleManagerService.GetRoles(function (data) {
               if (data) {
                   data.forEach(function (g) { g.stat = 'view'; });
                   $scope.Roles = data;
               }
           });

           RoleManagerService.GetControllers(function (data) {
               $scope.Controllers = data;
           });

           $scope.LoadActionRole = function (role) {
               $scope.selectedRole = role;
           }
           $scope.ItemClass = function (role) {
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

           //Update role's name only
           $scope.UpdateRole = function (role) {
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
                         return  g.Name != role.Name;
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

           $scope.AddActionsToRole = function (role) {
               $scope.Controllers.forEach(function (ctrl) {

               });
           }

           $scope.SelectAll = function ($event, ctrl) {
               if (ctrl.Actions && ctrl.Actions.length > 0) {
                   ctrl.Actions.forEach(function (action) {
                       action.Selected = $event.currentTarget.checked;
                   })
               }
           };
       }]);

    //Assign roles to action
    app.controller('ControllersCtrl', ['$scope', 'RoleManagerService',
        function ($scope, RoleManagerService) {

            $scope.selectedAction;
            $scope.selectedController;
            $scope.Roles = [];

            RoleManagerService.GetControllers(function (data) {
                $scope.Controllers = data;
                if ($scope.Controllers.length > 0) {
                    $scope.selectedController = $scope.Controllers[0];

                    if ($scope.Controllers[0].Actions.length > 0)
                        $scope.selectedAction = $scope.selectedController.Actions[0];

                    if ($scope.selectedAction != null)
                        $scope.LoadActionRoles($scope.selectedController, $scope.selectedAction);
                }
            });

            $scope.LoadActionRoles = function (ctrl, action) {
                $scope.selectedAction = action;
                $scope.selectedController = ctrl;
                RoleManagerService.GetActionRoles(action, function (data) {
                    $scope.Roles = data;
                })
            };
            $scope.SveActionRoles = function () {
                $scope.selectedAction.Roles = [];
                $scope.Roles.forEach(function (role) {
                    if (role.Selected) {
                        $scope.selectedAction.Roles.push(role);
                    }
                });
                RoleManagerService.SveActionRoles($scope.selectedAction);
            }
        }]);
})();