﻿'use strict';
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
    app.controller('ControllersCtrl', ['$scope', '$uibModal', '$log', 'RoleManagerService', function ($scope, $uibModal, $log, RoleManagerService) {
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
                $scope.selected = selectedItem;
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };
    }]);
    //Show roles and assign to an action
    app.controller('RolesCtrl', ['$scope', '$uibModalInstance', 'item',
        function ($scope, $uibModalInstance, item) {
            $scope.item = item;

            $scope.ok = function () {
                $uibModalInstance.close();
            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };
        }]);
})();