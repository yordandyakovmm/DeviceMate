(function() {
  'use strict';

  DeviceMateApp
    .controller('AccessorySearchCtrl', ['$scope', '$location', 'restSvc', 'configSvc', 'accessoryModel', '$stateParams', '$state', function($scope, $location, restSvc, configSvc, accessoryModel, $stateParams, $state) {
      //items Order
      $scope.itemsOrder = "asc";

      // currently loaded accessories
      $scope.accessories = [];

      // accessories pending submission:
      $scope.accessoriesForSubmission = [];

      //$scope.criteria = { offset: 0, limit: 20, SortColumns: 'DateTaken.DESC' };

      // count of the shown items (available, not available, all)
      $scope.totalCount = 0;
      $scope.availableCount = 0;
      $scope.notAvailableCount = 0;

      // the range of the shown items
      $scope.from = 0;
      $scope.to = 0;

      // all available sort columns for accessories
      $scope.accessorySortColumns = configSvc.getAccessorySortColumns();

      // sort column
      $scope.sortBy = "DateTaken";

      // Make sure we have a keyword property set if not inherited from parent controller:
      if (!$scope.criteria.Keyword) {
        $scope.criteria.Keyword = '';
      }

      $scope.historyShown = false;

      /**
       * Prepare accessory for submission - i.e. show form
       * @param  {Object} item The accessory object (see accessoryModel)
       * @return {NA}
       */
      $scope.prepSubmission = function(item) {

        // if the user has no team then set the id of the 'Other' team
        item.teamID = $scope.root.me.TeamId || $scope.getDefaultTeamId();

        if ($scope.accessoriesForSubmission.length < 4 && !$scope.historyShown) {
          var preventSub = false;

          //repeating devices prevention
          for (var i = 0; i < $scope.accessoriesForSubmission.length; i++) {
            if ($scope.accessoriesForSubmission[i] === item) {
              preventSub = true;
              break;
            }
          }

          if (!preventSub) {
            $scope.accessoriesForSubmission.push(item);
          }
        }
      };

      //Setting Accessory submiter team
      $scope.setTeam = function(team, accesory) {

        // store the previously selected team so it can be restored if cancel
        accesory.previousTeamID = accesory.teamID;
        accesory.teamID = team.Id;
      }

      /**
       * Remove accessory that has been added to the submission accessories array:
       * @param  {Object} item The accessory object (see accessoryModel)
       * @return {NA}
       */
      $scope.removeAccessory = function(item) {
        var itemFoundIndex = null;

        // restore the selected team
        if (item.previousTeamID) {
          item.teamID = item.previousTeamID;
          item.previousTeamID = "";
        }

        $scope.accessoriesForSubmission.forEach(function(singleItem, index) {
          if (singleItem.itemID === item.itemID) {
            itemFoundIndex = index;
          }
        });

        $scope.accessoriesForSubmission.splice(itemFoundIndex, 1);
      };


      /**
       * Submit accessory to server and refresh current list
       * @param  {Object} item The accessory object (see accessoryModel)
       * @return {NA}
       */
      $scope.submitAccessory = function(item) {
        var itemID = item.itemID,
          requestData = {
            isBusy: !item.IsBusy,
            Team: {
              Id: item.teamID
            }
          };

        restSvc
          .post('accessories', 'submit/' + itemID, requestData)
          .then(function() {

            // Remove accessory from submission array:
            $scope.removeAccessory(item);
            _loadAccessories();
          });
      };

      /**
       * Loads accessories from the previous page
       */
      $scope.loadPreviousAccessoriesPage = function() {

        // if we have previous pages
        if ($scope.criteria.offset > 0) {

          // change the criteria
          $scope.criteria.offset = $scope.criteria.offset - $scope.criteria.limit;

          // load the accessories from the previous page
          _loadAccessories();
        }
      };

      /**
       * Loads accessories from the next page
       */
      $scope.loadNextAccessoriesPage = function() {

        // if more pages exist
        if ($scope.totalCount > $scope.criteria.offset + $scope.criteria.limit) {

          // change the criteria
          $scope.criteria.offset = $scope.criteria.offset + $scope.criteria.limit;

          // load the accessories from the next page
          _loadAccessories();
        }
      };

      //update page
      $scope.updatePage = function() {
        _loadAccessories();
      }

      /**
       * Submit search by keyword if keyword is longer than 2 characters.
       * @return {NA}
       */
      $scope.submitAccessoriesSearchByKeyword = function() {

        // load accesories according to the selected filters
        _loadAccessories();
      };

      /**
       * Reset (remove) selected keyword in search box
       * @return {NA}
       */
      $scope.resetKeyword = function() {
        $scope.criteria.Keyword = '';

        // load accessories according to the selected filters
        _loadAccessories();
      };

      /**
       * Filters available/not available accessories.
       * @param {Object} event
       */
      $scope.filterByAvailabilityAccessories = function(event) {
        $scope.filterByAvailability(event);

        // load accessories according to the selected filters
        _loadAccessories();
      };

      /**
       * Sort accessories
       */
      $scope.sortAccessories = function() {
        var sortDirectionButton = angular.element(document.getElementById('sort-direction'));

        // if ascending option is selected
        if ($scope.itemsOrder == "asc") {
          $scope.criteria.SortColumns = $scope.sortBy + '.DESC';
        }

        // if descending option is selected
        if ($scope.itemsOrder == "desc") {
          $scope.criteria.SortColumns = $scope.sortBy + '.ASC';
        }

        // when a filter is changed we want to load the first page again
        $scope.criteria.offset = 0;

        // load accessories according to the selected filters
        _loadAccessories();
      }

      /**
       * Change sort direction
       * @param {Object} event
       */
      $scope.changeAccessoriesSortDirection = function(event) {
        // get the button element
        var sortDirectionButton = angular.element(event.currentTarget);

        // sort descending by the selected criteria
        if ($scope.itemsOrder == "asc") {

          $scope.criteria.SortColumns = $scope.sortBy + '.ASC';
          $scope.itemsOrder = "desc"
        }
        // sort ascending by the selected criteria
        else if ($scope.itemsOrder == "desc") {

          $scope.criteria.SortColumns = $scope.sortBy + '.DESC';
          $scope.itemsOrder = "asc"
        }

        // when a filter is changed we want to load the first page again
        $scope.criteria.offset = 0;

        // load accessories according to the selected filters
        _loadAccessories();
      }

      /**
       * Executes a request for loading accessories based on the current criteria
       */
      var _loadAccessories = function() {

        // load accessories
        restSvc.get('accessories', 'list', $scope.criteria).then(function(result) {
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
          $scope.accessoriesRangeList = [];
          while (myOffset < $scope.totalCount) {
            if ($scope.totalCount > (myOffset + $scope.criteria.limit)) {

              $scope.accessoriesRangeList.push({
                offset: myOffset,
                text: (myOffset + 1).toString() + '-' + (myOffset + $scope.criteria.limit)
              })
            } else {
              $scope.accessoriesRangeList.push({
                offset: myOffset,
                text: (myOffset + 1).toString() + '-' + $scope.totalCount
              })
            }
            myOffset += $scope.criteria.limit;
          }

          result = result.Accessories;
          $scope.accessories = [];
          angular.forEach(result, function(item) {
            $scope.accessories.push(new accessoryModel(item));
          });
        }, function(err) {

        });
      };

      $scope.showHistory = function(item) {
        $scope.currentAccessory = item;
        $scope.historyShown = true;
      };

      $scope.hideHistory = function() {
        $scope.historyShown = false;
      };

      // an event that is used to reload the accessories list when a new filter is selected
      $scope.$on('loadItems', function(event, clearSorting) {
        // reset sort by filter
        if (clearSorting) {
          $scope.sortBy = "DateTaken";
        }
        // load the accessories
        _loadAccessories();
      });

      //delete accesory
      $scope.deleteAccessory = function(device, event) {
        event.stopPropagation()

        if (window.confirm("Are you sure you want to Delete " + device.name + "? Deleting the accessory will also delete its history.")) {
          restSvc.get('accessories', 'remove', {
            "id": device.itemID
          }).then(function(data) {
            $state.transitionTo($state.current, $stateParams, {
              reload: true,
              inherit: false,
              notify: true
            });

            alert("Accessory Deleted")
          }, function(error) {
            alert("There was error while proceeding your request")
          });
        }
      }
      $scope.stopBubbling = function($event) {
        $event.cancelBubble = true;
      }

      // load the accessories
      _loadAccessories();
    }]);
})();
