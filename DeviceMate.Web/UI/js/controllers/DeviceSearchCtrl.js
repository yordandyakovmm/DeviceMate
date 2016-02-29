(function() {
  'use strict';

  DeviceMateApp
    .controller('DeviceSearchCtrl', ['$scope', '$location', 'restSvc', 'configSvc', 'deviceModel', 'userModel', '$stateParams', '$state', '$timeout',
      function($scope, $location, restSvc, configSvc, deviceModel, userModel, $stateParams, $state, $timeout) {
        //items Order
        $scope.itemsOrder = "asc";

        // currently loaded devices
        $scope.devices = [];

        // devices pending submission:
        $scope.devicesForSubmission = [];

        // device to show history for
        $scope.deviceHistory;
        $scope.historyShown = false;

        //$scope.criteria = { offset: 0, limit: 20, SortColumns: 'DateTaken.DESC' };

        // count of the shown items (available, not available, all)
        $scope.totalCount = 0;
        $scope.availableCount = 0;
        $scope.notAvailableCount = 0;

        //list of entities for the page range dropdown
        $scope.deviceRangeList = [];

        // the range of the shown items
        $scope.from = 0;
        $scope.to = 0;

        // all available sort columns for devices
        $scope.deviceSortColumns = configSvc.getDeviceSortColumns();

        // sort column
        $scope.sortBy = "DateTaken";
        //Setting Device submiter team

        $scope.setTeam = function(team, device) {
          device.teamID = team.Id;

          // store the previously selected team so it can be restored if cancel
          device.previousTeamID = device.teamID;
          device.teamID = team.Id;
        };

        /**
         * Prepare device for submission - i.e. show form
         * @param  {Object} item The device object (see deviceModel)
         * @return {NA}
         */
        $scope.prepSubmission = function(item) {
          // if the user has no team then set the id of the 'Other' team
          item.teamID = $scope.root.me.TeamId || $scope.getDefaultTeamId();
          item.first = true;

          //5th device prevention
          if ($scope.devicesForSubmission.length < 4 && !$scope.historyShown) {
            var preventSub = false;
            //repeating devices prevention
            for (var i = 0; i < $scope.devicesForSubmission.length; i++) {
              if ($scope.devicesForSubmission[i] === item) {
                preventSub = true;
                break;
              }
            }
            if (!preventSub) {
              $scope.devicesForSubmission.push(item);
            }
          }
        };

        /**
         * Remove device that has been added to the submission devices array:
         * @param  {Object} item The device object (see deviceModel)
         * @return {NA}
         */
        $scope.removeDevice = function(item) {
          var itemFoundIndex = null;
          // restore the selected team
          if (item.previousTeamID) {
            item.teamID = item.previousTeamID;
            item.previousTeamID = "";
          }

          $scope.devicesForSubmission.forEach(function(singleItem, index) {
            if (singleItem.itemID === item.itemID) {
              itemFoundIndex = index;
            }
          });

          $scope.devicesForSubmission.splice(itemFoundIndex, 1);
        };


        /**
         * Submit device to server and refresh current list
         * @param  {Object} item The device object (see deviceModel)
         * @return {NA}
         */
        $scope.submitDevice = function(item) {
          var itemID = item.itemID,
            requestData = {
              isBusy: !item.IsBusy,
              Team: {
                Id: item.teamID
              }
            };

          restSvc
            .post('devices', 'submit/' + itemID, requestData)
            .then(function() {

              // Remove device from submission array:
              $scope.removeDevice(item);

              _loadDevices();
            });
        };

        /**
         * Loads devices from the previous page
         */
        $scope.loadPreviousDevicesPage = function() {

          // if we have previous pages
          if ($scope.criteria.offset > 0) {

            // change the criteria
            $scope.criteria.offset = $scope.criteria.offset - $scope.criteria.limit;

            // load the devices from the previous page
            _loadDevices();
          }
        };

        /**
         * Loads devices from the next page
         */
        $scope.loadNextDevicesPage = function() {

          // if more pages exist
          if ($scope.totalCount > $scope.criteria.offset + $scope.criteria.limit) {

            // change the criteria
            $scope.criteria.offset = $scope.criteria.offset + $scope.criteria.limit;

            // load the devices from the next page
            _loadDevices();
          }
        };

        /**
         * Loads devices from the selected page
         */
        $scope.updatePage = function() {
          _loadDevices();
        };

        /**
         * Submit search by keyword if keyword is longer than 2 characters.
         * @return {NA}
         */
        $scope.submitDevicesSearchByKeyword = function() {
          // load devices according to the selected filters
          _loadDevices();
        };

        /**
         * Reset (remove) selected keyword in search box
         * @return {NA}
         */
        $scope.resetKeyword = function() {
          $scope.criteria.Keyword = '';

          // load devices according to the selected filters
          _loadDevices();
        };

        /**
         * Filters available/not available devices.
         * @param {Object} event
         */
        $scope.filterByAvailabilityDevices = function(event) {
          $scope.filterByAvailability(event);

          // load devices according to the selected filters
          _loadDevices();
        };

        /**
         * Sort devices
         */
        $scope.sortDevices = function() {
          var sortDirectionButton = angular.element(document.getElementById('sort-direction'));

          // if ascending option is selected
          if ($scope.itemsOrder == 'asc') {
            $scope.criteria.SortColumns = $scope.sortBy + '.DESC';
          }

          // if descending option is selected
          if ($scope.itemsOrder == 'desc') {
            $scope.criteria.SortColumns = $scope.sortBy + '.ASC';
          }

          // when a filter is changed we want to load the first page again
          $scope.criteria.offset = 0;

          // load devices according to the selected filters
          _loadDevices();
        };

        /**
         * Change sort direction
         * @param {Object} event
         */
        $scope.changeDevicesSortDirection = function(event) {
          // get the button element
          var sortDirectionButton = angular.element(event.currentTarget);

          // sort descending by the selected criteria
          if ($scope.itemsOrder == 'asc') {
            sortDirectionButton.removeClass('asc');
            sortDirectionButton.addClass('desc');
            $scope.criteria.SortColumns = $scope.sortBy + '.ASC';
            $scope.itemsOrder = "desc"
          }
          // sort ascending by the selected criteria
          else if ($scope.itemsOrder == 'desc') {
            sortDirectionButton.removeClass('desc');
            sortDirectionButton.addClass('asc');
            $scope.criteria.SortColumns = $scope.sortBy + '.DESC';
            $scope.itemsOrder = "asc"
          }

          // when a filter is changed we want to load the first page again
          $scope.criteria.offset = 0;

          // load devices according to the selected filters
          _loadDevices();
        };

        /**
         * Executes a request for loading devices based on the current criteria
         */
        var _loadDevices = function() {

          // load devices
          restSvc.get('devices', 'list', $scope.criteria).then(function(result) {
            $scope.totalCount = result.TotalItems;
            $scope.availableCount = result.AvailableCount;
            $scope.notAvailableCount = result.TotalItems - result.AvailableCount;

            // determine the range of the loaded items
            $scope.from = $scope.criteria.offset + 1;

            // if the total items count is reached
            if ($scope.totalCount < ($scope.criteria.offset + $scope.criteria.limit)) {
              $scope.to = $scope.totalCount;
            } else {
              $scope.to = $scope.criteria.offset + $scope.criteria.limit;
            }

            var myOffset = 0;

            //creating pagination dropdown list items
            $scope.deviceRangeList = [];

            while (myOffset < $scope.totalCount) {
              if ($scope.totalCount > (myOffset + $scope.criteria.limit)) {
                $scope.deviceRangeList.push({
                  offset: myOffset,
                  text: (myOffset + 1).toString() + '-' + (myOffset + $scope.criteria.limit)
                })
              } else {
                $scope.deviceRangeList.push({
                  offset: myOffset,
                  text: (myOffset + 1).toString() + '-' + $scope.totalCount
                })
              }
              myOffset += $scope.criteria.limit;
            }

            result = result.Devices;
            $scope.devices = [];
            angular.forEach(result, function(item) {
              $scope.devices.push(new deviceModel(item));
            });
          }, function(err) {

          });
        };

        $scope.showHistory = function(item) {
          $scope.currentDevice = item;
          $scope.historyShown = true;
        }

        $scope.hideHistory = function() {
          $scope.historyShown = false;
        }

        // an event that is used to reload the devices list when a new filter is selected
        $scope.$on('loadItems', function(event, clearSorting) {

          // reset sort by filter
          if (clearSorting) {
            $scope.sortBy = "DateTaken";
          }

          // load the devices
          _loadDevices();
        });

        $scope.deleteDevice = function(device, event) {
          console.log(device.itemID)
          event.stopPropagation()

          if (window.confirm("Are you sure you want to Delete " + device.name + "? Deleting the device will also delete its history.")) {
            restSvc.get('devices', 'remove', {
              "id": device.itemID
            }).then(function(data) {
              $state.transitionTo($state.current, $stateParams, {
                reload: true,
                inherit: false,
                notify: true
              });

              alert("Device Deleted")
            }, function(error) {
              alert("There was error while proceeding your request")
            });
          }
        }

        $scope.stopBubbling = function($event) {
          $event.cancelBubble = true;
        }

        // load the devices
        $scope.currentUser = userModel.getUsers;
        _loadDevices();
      }
    ]);
})();
