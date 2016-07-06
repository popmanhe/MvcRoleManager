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
        })
        .state('roleuser', {
            url: "/roleuser",
            templateUrl: basePath + "RoleUser.html",
            controller: 'RoleUserCtrl'
        });
    }]);

    //tabs' controller
    app.controller('TabsCtrl', ['$scope', function ($scope) {
        $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
            $scope.tabs.forEach(function (tab) {
                if (tab.link === '#/' + toState.name)
                    $scope.selectedTab = tab;
            });
        });

        $scope.tabs = [
            { link: '#/', label: 'Roles->Action' },
            { link: '#/actionrole', label: 'Actions -> Role' },
            { link: '#/userrole', label: 'Users -> Role' },
            { link: '#/roleuser', label: 'Roles -> User' }
        ];
        $scope.selectedTab = $scope.tabs[0];
        $scope.setSelectedTab = function (tab) {
            $scope.selectedTab = tab;
        }

        $scope.tabClass = function (tab) {
            if ($scope.selectedTab === tab) {
                return "active";
            } else {
                return "";
            }
        }
    }]);

    //Assign roles to action
    app.controller('RoleActionCtrl', ['$scope', 'RoleManagerService',
        function ($scope, RoleManagerService) {
            //set up default properties for controller directive
            $scope.Properties = {
                ShowCheckbox: false,
                SelectFirstItem: true
            };

            $scope.selectedAction;
            $scope.GetRolesByAction = function (action) {
                if (action)
                    $scope.selectedAction = action;
                $scope.Properties.AssignedTo = $scope.selectedAction.ControllerName + '.' + $scope.selectedAction.ActionName;
                //only return selected roles' ids
                RoleManagerService.GetRolesByAction($scope.selectedAction, function (data) {
                    if (data && data.length > 0) {
                        $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                            role.Selected = data.indexOf(role.Id) > -1;
                            return role;
                        });
                    }
                });
            };

            $scope.AddRolesToAction = function (roles) {
                $scope.selectedAction.Roles = roles.filter(function (role) {
                    return role.Selected;
                });

                RoleManagerService.AddRolesToAction($scope.selectedAction);
            }
        }]);

    //Assign actions to role
    app.controller('ActionRoleCtrl', ['$scope', '$document', 'RoleManagerService',
       function ($scope, $document, RoleManagerService) {
           //set up default properties for controller directive
           $scope.Properties = {
               ShowCheckbox: true,
               SelectFirstItem: false
       };

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
    app.controller('UserRoleCtrl', ['$scope', 'RoleManagerService',
     function ($scope, RoleManagerService) {
         //default for user directive
         $scope.Properties = {
             ShowCheckbox: true
         };


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

    //Assign roles to user
    app.controller('RoleUserCtrl', ['$scope', 'RoleManagerService',
     function ($scope, RoleManagerService) {

         var self = this;

         var selectedUser = null;
         //default settings for user directive
         $scope.Properties = {
             ShowCheckbox: false,
             SelectFirstItem: true
         };


         $scope.AddRolesToUser = function (selectedRoles) {
             selectedUser.Roles = selectedRoles;
            
             RoleManagerService.AddRolesToUser(selectedUser);
         }

         $scope.GetRolesByUser = function (user) {
             self.selectedUser = user;
             RoleManagerService.GetRolesByUser(user, function (data) {
                 if (data) {
                     $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                         role.Selected = data.indexOf(role.Id) > -1;
                         return role;
                     });
                 }
             });
         }

     }]);
})();