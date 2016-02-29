(function() {
  'use strict';

  DeviceMateApp
    .provider('restSvc', [function() {

      this.$get = ['$q', '$http', '$location',
        function($q, $http, $location) {

          return {

            /**
             * Executes a server request.
             * @param {type} controller
             * @param {type} method
             * @param {type} params
             * @returns {ajax_L9.ajaxAnonym$1.get@call;promise.promise}
             */
            get: function(controller, method, params) {
              var deferred = $q.defer();

              // if no params are passed then we use an empty object
              params = params || {};

              $http.get('http://' + $location.host() + ':' + $location.port() + '/api/v1/' + controller + '/' + method, {
                'params': params
              }).then(function(result) {
                deferred.resolve(result.data);
              }, function(err) {
                console.log('Error', err);
                deferred.reject(err);
              });

              return deferred.promise;
            },

            post: function(controller, method, params) {
              var deferred = $q.defer();

              // if no params are passed then we use an empty object
              params = params || {};

              $http.post('http://' + $location.host() + ':' + $location.port() + '/api/v1/' + controller + '/' + method, params).then(function(result) {
                deferred.resolve(result.data);
              }, function(err) {
                console.log('Error', err);
                deferred.reject(err);
              });

              return deferred.promise;
            }
          };
        }
      ];
    }]);
})();
