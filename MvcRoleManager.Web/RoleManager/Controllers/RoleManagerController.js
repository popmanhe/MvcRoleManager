'use strict';
; (function () {
    var app = angular.module('RoleManager');
    var basePath = '/RoleManager/partials/';
    //Config routes.
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/");
        //
        // Now set up the states
        $stateProvider
          .state('controllers', {
              url: "/",
              templateUrl: basePath + "Controllers.html",
              controller: 'ControllersCtrl'
          });
    }]);
    //Get all controllers and actions
    app.controller('ControllersCtrl', ['$scope', '$uibModal', '$log', 'RoleManagerService',
        function ($scope, $uibModal, $log, RoleManagerService) {
        RoleManagerService.getControllers(function (data) {
            $scope.Controllers = data;
        });

        $scope.assigntoRoles = function (ctrl, act) {
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: true,
                templateUrl: basePath + 'roles.html',
                controller: 'RolesCtrl',
                size: 'lg',
                resolve: {
                    item: function () {
                        return { controller: ctrl, action: act };
                    }
                }
            });


            modalInstance.result.then(function (selectedItem) {
               // $scope.selected = selectedItem;
            }, function () {
               
            });
        };
    }]);
    //Show roles and assign to an action
    app.controller('RolesCtrl', ['$scope', '$uibModalInstance', 'item', 'RoleManagerService',
        function ($scope, $uibModalInstance, item, RoleManagerService) {
            $scope.item = item;
            $scope.roles = [];
            //$scope.getRoles = function () {
            RoleManagerService.GetActionRoles(item.action, function (result) {
                $scope.roles = result.data;
            });
            //};
            $scope.ok = function () {
                var $this = this;
                if ($this.item.action.modified) {
                    $this.item.action.Roles = [];
                    $this.roles.forEach(function (role) {
                        if (role.Selected) {
                            $this.item.action.Roles.push(role);
                        }
                    });

                    RoleManagerService.SveActionRoles($scope.item.action);

                }
                $uibModalInstance.close();
            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }]);
})();