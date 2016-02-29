(function() {
  var DeviceMateApp = angular.module('DeviceMateApp', [
    'ui.router',
    'angularMoment',
    'devicemateDropdown',
    'ngDialog'
  ]).constant('angularMomentConfig', {
    // optional
    timezone: 'Europe/London' // optional
  });

  /*DeviceMateApp
      .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider', '$compileProvider',
          function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider, $compileProvider) {

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
                      controller: 'SearchCtrl'
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
                  });

              $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|skype):/);
          }
      ]);*/

  window.DeviceMateApp = DeviceMateApp;
})();
