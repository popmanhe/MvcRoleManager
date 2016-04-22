'use strict';
; (function () {
    var app = angular.module('RoleManager');
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/");
        //
        // Now set up the states
        $stateProvider
          .state('controllers', {
              url: "/",
              templateUrl: "/RoleManager/partials/Controllers.html",
              controller: 'ControllersCtrl'
          });
    }]);

    app.controller('ControllersCtrl', function ($scope) {
        $scope.text = "hello world!";
    });
})();