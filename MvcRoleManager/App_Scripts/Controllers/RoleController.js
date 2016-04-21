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
              url: "/profile",
              templateUrl: "/Security/RoleManager/PartialIndex",
              controller: 'ProfileController'
          })
        .state('login', {
            url: "/login",
            templateUrl: "/www/partials/Account/index.html",
            controller: 'LoginController'
        })
        .state('register', {
            url: "/register",
            templateUrl: "/www/partials/Account/index.html",
            controller: 'RegisterController'
        });
    }]);

    app.controller('ProfileController', []);
})();