﻿'use strict';
; (function () {
    var app = angular.module('');

    app.config(['$provide', function ($provide) {
        $provide.factory('accountService', ['$http', function ($http) {

            var service = {};

            var showJSONMessage = function (msg) {
                alert(JSON.stringify(msg));
            };

            service.showJSONMessage = showJSONMessage;

            service.getUserProfile = function (callback) {
                $http.get('/api/Account/GetUserProfile').then(
                    function (result) {//success
                        callback(result.data);
                  }
                , function () {//failed

                });
            }

            service.login = function (user, callback) {
                $http.post('/api/Account/Login', user)
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
            };

            service.register = function (user, callback) {
                $http.post('/api/Account/Register', user)
                .then(
                function (result) {
                    callback(result);
                },
                function () { }
                )
            }

            service.saveUserProfile = function () { };

            return service;

        }]);
    }]);

})();