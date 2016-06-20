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
        .state('actiongroup', {
            url: "/actiongroup",
            templateUrl: basePath + "ActionGroup.html",
            controller: 'ActionGroupCtrl'
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
            { link: '#/actiongroup', label: 'Action Group' }
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

    //Group actions to provide a logic action
    app.controller('ActionGroupCtrl', ['$scope', '$log', 'RoleManagerService',
       function ($scope, $log, RoleManagerService) {
           $scope.selectedGroup;
           $scope.Controllers = [];
           $scope.Groups = [];

           RoleManagerService.GetGroups(function (data) {
               $scope.Groups = data;
           });

           RoleManagerService.GetControllers(function (data) {
               $scope.Controllers = data;
           });

           $scope.LoadActionGroup = function (group) {
               $scope.selectedGroup = group;
           }
           $scope.ItemClass = function (group) {
               if ($scope.selectedGroup == group) {
                   return "active";
               } else {
                   return "";
               }
           }
           $scope.EditGroup = function (group) {
               group.stat = 'edit';
           }

           //Update group's name and description only
           $scope.UpdateGroup = function (group) {
               RoleManagerService.UpdateGroup(group, function () {
                   group.stat = 'view';
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

    //Assign roles to actions
    app.controller('ControllersCtrl', ['$scope', '$log', 'RoleManagerService',
        function ($scope, $log, RoleManagerService) {

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
                RoleManagerService.GetActionRoles(action, function (result) {
                    $scope.Roles = result.data;
                })
            };
            $scope.SveActionRoles = function () {
                $scope.selectedAction.Roles = [];
                $scope.roles.forEach(function (role) {
                    if (role.Selected) {
                        $scope.selectedAction.Roles.push(role);
                    }
                });
                RoleManagerService.SveActionRoles($scope.selectedAction);
            }
        }]);
})();