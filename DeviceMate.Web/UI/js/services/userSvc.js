(function() {
  'use strict';

  DeviceMateApp
    .factory('userSvc', ['$q', 'restSvc', 'userModel',
      function($q, restSvc, userModel) {
        var modelUser;

        return {

          /**
           * Executes a server request to load the user data.
           */
          getUserInfoFromServer: function() {
            var deferred = $q.defer();

            if (modelUser) {
              deferred.resolve(modelUser);
            } else {

              // Fetch user data
              restSvc
                .get('users', 'me')
                .then(function(currentUser) {
                  modelUser = new userModel(currentUser);
                  deferred.resolve(modelUser);
                }, function(err) {
                  deferred.reject(err);
                });
            }

            return deferred.promise;
          },

          /**
           * Returns the user model.
           */
          getUserModel: function() {
            return modelUser;
          }
        };
      }
    ]);
})();
