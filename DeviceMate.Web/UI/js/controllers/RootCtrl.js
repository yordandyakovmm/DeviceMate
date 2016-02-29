(function() {
  'use strict';

  DeviceMateApp
    .controller('RootCtrl', ['$scope', 'userSvc', 'userModel', function($scope, userSvc, userModel) {
      $scope.root = {

        // flags that control the visibility of the dropdowns in the header
        showManageNav: false,
        showUserNav: false,
        manageDropdown: [{
          text: 'Add Device',
          value: 1,
          options: {
            href: "/device/AddEdit"
          }
        }, {
          text: 'Add Accessory',
          value: 2,
          options: {
            href: '/Accessory/AddEdit'
          }
        }]
      };

      // Fetch user data:
      userSvc.getUserInfoFromServer()
        .then(function(modelUser) {
          $scope.root.me = modelUser;
        });
    }]);
})();
