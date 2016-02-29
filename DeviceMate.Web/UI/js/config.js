(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider', '$compileProvider',
      function($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider, $compileProvider) {

        // Set default route
        $urlRouterProvider.otherwise("/search/devices");

        // Routing table:
        $stateProvider
          .state('home', {
            url: "/home",
            templateUrl: 'UI/templates/home.html',
            controller: 'HomeCtrl'
          })
          .state('search', {
            abstract: true,
            url: "/search",
            templateUrl: 'UI/templates/search.html',
            controller: 'SearchCtrl',
            resolve: {
              /** Fix: Not minification safe */
              userInfo: function(userSvc) {
                return userSvc.getUserInfoFromServer();
              }
            }
          })
          .state('search.devices', {
            url: "/devices",
            templateUrl: 'UI/templates/search_devices.html',
            controller: 'DeviceSearchCtrl'
          })
          .state('search.accessories', {
            url: "/accessories",
            templateUrl: 'UI/templates/search_accessories.html',
            controller: 'AccessorySearchCtrl'
          })
          .state('editdevice', {
            url: "/editdevice/:deviceID",
            templateUrl: 'UI/templates/edit_device.html',
            controller: 'DeviceEditCtrl',
            resolve: {
              /** Fix: Not minification safe */
              userInfo: function(userSvc) {
                return userSvc.getUserInfoFromServer();
              }
            }
          })
          .state('accessory', {
            url: "/accessory/:accessoryId",
            templateUrl: 'UI/templates/accessory.html',
            controller: 'AccessoryCtrl',
            controllerAs: 'vm',
            resolve: {
              /** Fix: Not minification safe */
              userInfo: function(userSvc) {
                return userSvc.getUserInfoFromServer();
              }
            }
          });

        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|skype):/);
      }
    ]);
})();
