;(function(){
    var app = angular.module('RoleManager');
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/");
        //
        // Now set up the states
        $stateProvider
          .state('controlleraction', {
              url: "/",
              templateUrl: "/RoleManager/partials/Controllers.html",
              controller: 'ProfileController'
          });
    }]);

    app.controller('ProfileController', function ($scope) {
        $scope.text = "hello world!";
    });
})();