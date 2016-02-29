(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .controller('AccessoryCtrl', AccessoryCtrl);

  AccessoryCtrl.$inject = ['$scope', '$timeout', '$state', 'userInfo', 'filterSvc', 'modalSvc', 'accessorySvc', 'FilterModel'];

  function AccessoryCtrl($scope, $timeout, $state, userInfo, filterSvc, modalSvc, accessorySvc, FilterModel) {
    var vm = this;

    /** Couldn't use the accessoryModel model because of inconsistencies */
    vm.accessory = {
      Holder: {
        Email: userInfo.Email,
        IsBusy: true,
        Team: {
          Id: userInfo.TeamId,
        },
        Location: {
          Id: userInfo.Location ? userInfo.Location.Id : ''
        }
      }
    };

    vm.filters = new FilterModel();
    vm.openAddFilterDialog = openAddFilterDialog;
    vm.openDeleteFilterDialog = openDeleteFilterDialog;
    vm.saveAccessory = saveAccessory;

    activate();

    function activate() {
      getFilters();
      getCurrentAccessory();
    }

    function getCurrentAccessory() {
      /** If we are editing the device we should have it's id */
      if ($state.params.accessoryId) {
        accessorySvc.getAccessory($state.params.accessoryId)
          .then(function(result) {
            vm.accessory = result;
          });
      }
    }

    function getFilters() {
      filterSvc.getAllAccessories()
        .then(onGetAllAccessoriesSuccess);

      function onGetAllAccessoriesSuccess(result) {
        vm.filters = result;
      }
    }

    function openAddFilterDialog(filterType, filterName) {
      filterSvc.saveFilter(filterType, filterName)
        .then(getFilters)
        .catch(onSaveFilterError);

      /** Log or notify the user (i.e. toastr) for the rejection */
      function onSaveFilterError(reject) {
        console.log(reject);
      }
    }

    function openDeleteFilterDialog(filterType, filterId) {
      filterSvc.deleteFilter(filterType, filterId)
        .then(getFilters);
    }

    function saveAccessory() {
      accessorySvc.saveAccessory(vm.accessory)
        .then(onSaveAccessorySuccess)
        .catch(onSaveAccessoryError);

      /** Optionally notify user of the operation's success */
      function onSaveAccessorySuccess(result) {
        $timeout(function() {
          $state.reload();
        }, 1000);
      }

      /** Log or notify the user (i.e. toastr) for the rejection */
      function onSaveAccessoryError(reject) {
        console.log(reject);
      }
    }
  }
})();
