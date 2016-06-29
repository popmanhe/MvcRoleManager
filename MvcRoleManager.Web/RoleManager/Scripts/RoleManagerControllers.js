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
          .state('roleaction', {
              url: "/",
              templateUrl: basePath + "RoleAction.html",
              controller: 'RoleActionCtrl'
          })
        .state('actionrole', {
            url: "/actionrole",
            templateUrl: basePath + "ActionRole.html",
            controller: 'ActionRoleCtrl'
        })
        .state('userrole', {
            url: "/userrole",
            templateUrl: basePath + "UserRole.html",
            controller: 'UserRoleCtrl'
        });
    }]);

    //tabs' controller
    app.controller('TabsCtrl', ['$scope', function ($scope) {
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
            $scope.tabs.forEach(function (tab) {
                if (tab.link == '#/' + toState.name)
                    $scope.selectedTab = tab;
            });
        });

        $scope.tabs = [
            { link: '#/', label: 'Roles->Action' },
            { link: '#/actionrole', label: 'Actions -> Role' },
            { link: '#/userrole', label: 'Users <-> Role' }
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

    //Assign roles to action
    app.controller('RoleActionCtrl', ['$scope', 'RoleManagerService',
        function ($scope, RoleManagerService) {

            $scope.selectedAction;
            $scope.selectedController;
            $scope.Roles = [];
            $scope.GetRolesByAction = function (action) {
                $scope.selectedAction = action;

                RoleManagerService.GetRolesByAction(action, function (data) {
                    $scope.Roles = data;
                });
            };

            $scope.AddRolesToAction = function () {
                $scope.selectedAction.Roles = [];
                $scope.Roles.forEach(function (role) {
                    if (role.Selected) {
                        $scope.selectedAction.Roles.push(role);
                    }
                });
                RoleManagerService.AddRolesToAction($scope.selectedAction);
            }
        }]);

    //Assign actions to role
    app.controller('ActionRoleCtrl', ['$scope', '$document', 'RoleManagerService',
       function ($scope, $document, RoleManagerService) {
           
           $scope.GetActionsByRole = function (role) {
               RoleManagerService.GetActionsByRole(role, function (data) {
                   $scope.Methods.SetSelectedActions(data);
               });
           }

           $scope.AddActionsToRole = function (role) {
               if (role)
                   $scope.selectedRole = role;

               $scope.selectedRole.Actions = [];
               $scope.Properties.Controllers.forEach(function (ctrl) {
                   ctrl.Actions.forEach(function (action) {
                       if (action.Selected) {
                           action.ControllerName = ctrl.ControllerName;
                           $scope.selectedRole.Actions.push(action);
                       }
                   });
               });
               RoleManagerService.AddActionsToRole($scope.selectedRole, function (data) {

               });
           }
       }]);

    //Assign users to role
    app.controller('UserRoleCtrl', ['$scope', '$document', 'RoleManagerService',
     function ($scope, $document, RoleManagerService) {
         $scope.AddUsersToRole = function (role) {
             role.Users = [];
             $scope.Properties.Users.forEach(function (user) {
                 if (user.Selected)
                     role.Users.push(user);
             });
             RoleManagerService.AddUsersToRole(role);
         }

         $scope.GetUsersByRole = function (role) {
             RoleManagerService.GetUsersByRole(role, function (data) {
                 $scope.Methods.SetSelectedUsers(data);
             });
         }

     }]);
})();