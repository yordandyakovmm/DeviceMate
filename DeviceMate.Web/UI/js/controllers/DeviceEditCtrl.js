(function() {
  'use strict';

  DeviceMateApp
    .controller('DeviceEditCtrl', ['$scope', '$stateParams', '$state', 'ngDialog', 'modalSvc', 'userSvc', 'restSvc', 'deviceModel', function($scope, $stateParams, $state, ngDialog, modalSvc, userSvc, restSvc, deviceModel) {
      var userModel = userSvc.getUserModel();

      // flag used to detect if editing an existing device or creating a new one
      var isNew = !$stateParams.deviceID;
      if (isNew) {
        $scope.title = "ADD NEW DEVICE";
      } else {
        $scope.title = "EDIT DEVICE";
      }

      $scope.showSuccessNotification = false;
      $scope.successNotification = "You have successfully edited a device!";

      $scope.filters = {};

      //initial device object
      $scope.device = {
        "Holder": {
          "Email": userModel.Email,
          "IsBusy": true,
          "Team": {
            "Id": userModel.TeamId,
          },
          "Location": {
            "Id": userModel.Location ? userModel.Location.Id : ''
          }
        },
        "Model": {
          Id: ""
        },
        "Manufacturer": {
          Id: ""
        },
        "Type": {
          Id: "",
        },
        "Color": {
          Id: ""
        },
        "ScreenSize": {
          Id: ""
        },
        "Resolution": {
          "Width": {
            Id: ""
          },
          "Height": {
            Id: ""
          }
        },
        "Platform": {
          Id: ""
        }
      };

      // newly created filters
      $scope.newColorName = '';
      $scope.newScreenSizeName = '';
      $scope.newResolutionHeightName = '';
      $scope.newResolutionWidthName = '';
      $scope.newOsName = '';
      $scope.newManufacturerName = '';
      $scope.newModelName = '';

      restSvc.get('filters', 'devices/all').then(function(result) {

        // expose the filters to the template
        $scope.filters = result;

        //first manufacturers and models are empty
        $scope.filters.Manufacturers = [];
        $scope.filters.Models = [];

        // check if we try to edit an existing device or create a new one
        // if editing then load the data for the device as well
        if (!isNew) {
          restSvc.get('devices', 'show/' + $stateParams.deviceID).then(function(result) {
            // check for errors
            if (result.Message != "OK") {
              console.log('Error in device loading!');
            }

            delete result.Message;

            //todo: Uncomment this when the fields names in the device model are fixed to match the expectiong ones in the request for device saving
            // expose the device object to the template
            //$scope.device = new deviceModel(result);
            $scope.device = result;

            //populate the list of manufacturers for the device's OS
            $scope.osChanged(result.Platform);
            $scope.manufacturerChanged(result.Manufacturer);
          }, function(err) {
            console.log('Error', err);
          });
        }
      });

      // save a device
      $scope.saveDevice = function() {
        //* @todo Validate $scope.device for OS, Manufacturer and Model as they are required */
        restSvc.post('devices', 'save', $scope.device).then(function() {

          // show the success notification for creating/editing a device
          $scope.showSuccessNotification = true;

          // if a new device has been created then change the message
          if (isNew) {
            $scope.successNotification = "You have successfully added a device!";
          }

          // reload the page
          setTimeout(function() {
            $state.reload();
          }, 1000);

        });
      };

      // populate the manufacturers list when the selected OS is changed
      $scope.osChanged = function(os) {
        var id = os.Id;

        for (var i = 0; i < $scope.filters.Platforms.length; i++) {
          if ($scope.filters.Platforms[i].Id == id) {
            $scope.filters.Manufacturers = $scope.filters.Platforms[i].Manufacturers;
          }
        }

        // reset the models
        $scope.filters.Models = [];
      };

      // populate the models list when the selected manufacturer is changed
      $scope.manufacturerChanged = function(manufacturer) {
        var id = manufacturer.Id;

        for (var i = 0; i < $scope.filters.Manufacturers.length; i++) {
          if ($scope.filters.Manufacturers[i].Id == id) {
            $scope.filters.Models = $scope.filters.Manufacturers[i].Models;
          }
        }
      };

      var reloadFilters = function() {
        restSvc.get('filters', 'devices/all').then(function(result) {

          // expose the filters to the template
          $scope.filters = result;

          //first manufacturers and models are empty
          $scope.filters.Manufacturers = [];
          $scope.filters.Models = [];

          // populate the list of manufacturers
          $scope.osChanged($scope.device.Platform);

          // populate the list of models
          $scope.manufacturerChanged($scope.device.Manufacturer);
        });
      };

      /** @todo: Validation needs refactoring (alert...?) */
      $scope.addFilter = function(filterName, url, fieldName) {
        var name = $scope["new" + filterName + "Name"];

        // if a filter name is entered
        if (name) {
          var params = {};
          params[fieldName] = name;

          // for creating a new manufacturer the os id needs to be sent to the server as well
          if (filterName == "Manufacturer") {
            var osId = angular.element(document.querySelector("#os-dropdown")).val();

            // check if an OS is selected
            if (osId) {
              params.OsId = osId;
            }
            // if no OS selected then notify the user
            else {
              alert("Please select an OS!");
              return;
            }
          }

          // for creating a new model the manufacturer id needs to be sent to the server as well
          if (filterName == "Model") {
            var manufacturerId = angular.element(document.querySelector("#manufacturer-dropdown")).val();

            // check if a manufacturer is selected
            if (manufacturerId) {
              params.ManufacturerId = manufacturerId;
            }
            // if no manufacturer selected then notify the user
            else {
              alert("Please select a manufacturer!");
              return;
            }
          }

          // execute a request for saving the new filter value
          restSvc.post(url, 'save', params).then(function() {
            //when the new value is saved close the dialog
            $scope['showAdd' + filterName + 'Dialog'] = false;

            // reload the filters so the new one will appear in the list
            reloadFilters();
            modalSvc.closeAll();
          }, function(err) {
            console.log('Error in filter saving', err);
          });
        }
        // if no name then notify the user
        else {
          alert('Please enter a filter name!');
        }
      };

      $scope.deleteFilter = function(filter, url) {
        var modalOptions = {
          message: 'Are you sure you want to delete <strong>' + filter.Name + '</strong>'
        };

        modalSvc.confirm(modalOptions)
          .then(onUserConfirmSuccess)
          .then(onDeleteFilterSuccess);

        function onUserConfirmSuccess() {
          return restSvc.get(url, 'delete/' + filter.Id);
        }

        function onDeleteFilterSuccess() {
          reloadFilters();
        }
      };

      $scope.openAddFilterDialog = function(template) {
        var dialogOptions =  {
          template: template,
          controller: null,
          scope: $scope
        };

        modalSvc.alert(dialogOptions);

        // reset the values
        $scope.newColorName = '';
        $scope.newScreenSizeName = '';
        $scope.newResolutionHeightName = '';
        $scope.newResolutionWidthName = '';
        $scope.newOsName = '';
        $scope.newManufacturerName = '';
        $scope.newModelName = '';
      };
    }]);
})();
