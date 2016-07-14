'use strict';
(function () {
    var app = angular.module('RoleManager');
    var basePath = virtualPath + '/RoleManager/Home/';
    //Config routes.
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/");

        // Now set up the states
        $stateProvider
          .state('roleaction', {
              url: "/",
              templateUrl: basePath + "RoleAction",
              controller: 'RoleActionCtrl'
          })
        .state('actionrole', {
            url: "/actionrole",
            templateUrl: basePath + "ActionRole",
            controller: 'ActionRoleCtrl'
        })
        .state('userrole', {
            url: "/userrole",
            templateUrl: basePath + "UserRole",
            controller: 'UserRoleCtrl'
        })
        .state('roleuser', {
            url: "/roleuser",
            templateUrl: basePath + "RoleUser",
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
        };

        $scope.tabClass = function (tab) {
            if ($scope.selectedTab === tab) {
                return "active";
            } else {
                return "";
            }
        };
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
                RoleManagerService.GetRolesByAction($scope.selectedAction)
                .then(
                    function (result) {
                        var data = result.data;
                        if (data && data.length > 0) {
                            data = data.map(function (role) {
                                return role.Id;
                            });
                            $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                                role.Selected = data.indexOf(role.Id) > -1;
                                return role;
                            });
                        }
                        else {//no role selected
                            $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                                role.Selected = false;
                                return role;
                            });
                        }
                    },
                    function () { }
               );
            };

            $scope.AddRolesToAction = function (roles) {
                $scope.selectedAction.Roles = roles.filter(function (role) {
                    return role.Selected;
                });

                RoleManagerService.AddRolesToAction($scope.selectedAction)
                .then(
                    function (result) {

                    },
                    function () { }
                );
            };
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
               RoleManagerService.GetActionsByRole(role).then(
                   function (result) {//succeeded
                       $scope.ActionMethods.SetSelectedActions(result.data);
                   },
                   function (result) {//failed
                   }
               );
           };

           $scope.AddActionsToRole = function (role, success, failed) {
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

               RoleManagerService.AddActionsToRole($scope.selectedRole)
               .then(
                    function (result) {//succeeded
                        $scope.RoleMethods.SetMessage({ 'Type': 'success', 'Content': 'Save Succeeded.' });
                    },
                    function (result) {//failed
                        $scope.RoleMethods.SetMessage({ 'Type': 'danger', 'Content': 'Save failed.' });
                    }
            );
           };
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
             RoleManagerService.AddUsersToRole(role)
              .then(
                    function (result) {//succeeded
                        $scope.RoleMethods.SetMessage({ 'Type': 'success', 'Content': 'Save Succeeded.' });
                    },
                    function (result) {//failed
                        $scope.RoleMethods.SetMessage({ 'Type': 'danger', 'Content': 'Save failed.' });
                    }
            );
         };

         $scope.GetUsersByRole = function (role) {
             RoleManagerService.GetUsersByRole(role)
             .then(
                function (result) {
                    $scope.UserMethods.SetSelectedUsers(result.data);
                },
                function () { }
            );
         };

     }]);

    //Assign roles to user
    app.controller('RoleUserCtrl', ['$scope', 'RoleManagerService',
     function ($scope, RoleManagerService) {

         var self = this;

         this.selectedUser = null;
         //default settings for user directive
         $scope.Properties = {
             ShowCheckbox: false,
             SelectFirstItem: true
         };


         $scope.AddRolesToUser = function (selectedRoles) {
             self.selectedUser.Roles = selectedRoles;

             RoleManagerService.AddRolesToUser(self.selectedUser)
             .then(
                function (result) {

                },
                function () { }
                );
         };

         $scope.GetRolesByUser = function (user) {
             if (user)
                 self.selectedUser = user;
             RoleManagerService.GetRolesByUser(self.selectedUser)
              .then(
                function (result) {
                    if (result.data) {
                        $scope.Properties.Roles = $scope.Properties.Roles.map(function (role) {
                            role.Selected = result.data.indexOf(role.Id) > -1;
                            return role;
                        });
                    }
                },
                function () { }
            );
         };

     }]);
})();