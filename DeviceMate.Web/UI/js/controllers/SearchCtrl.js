(function() {

  'use strict';

  DeviceMateApp
    .controller('SearchCtrl', ['$scope', '$location', 'userSvc', 'restSvc', 'teamModel', 'configSvc', function($scope, $location, userSvc, restSvc, teamModel, configSvc) {

      // mapping object for the filters labels
      var filterLabelsMapping = {
        Cities: 'City',
        Colors: 'Color',
        Platforms: 'OS',
        Manufacturers: 'Manufacturer',
        Models: 'Model',
        ScreenHeight: 'Height (px)',
        ScreenWidth: 'Width (px)',
        Teams: 'Team',
        Descriptions: 'Description'
      };

      // mapping object for the filter keys that are used in the criteria object
      var filterParamsMapping = {
        Cities: 'TownId',
        Colors: 'ColorId',
        Platforms: 'OsId',
        Manufacturers: 'ManufacturerId',
        Models: 'ModelId',
        ScreenHeight: 'ResolutionHeightId',
        ScreenWidth: 'ResolutionWidthId',
        Teams: 'TeamId',
        Descriptions: 'DescriptionId'
      };

      // a global (in this controller) variable that holds the devices filters
      var deviceFiltersObject = {};

      $scope.criteria = {
        offset: 0,
        limit: 20,
        SortColumns: 'DateTaken.DESC',
        Keyword: '',

        // by default filter devices/accessories based on user's location
        TownId: userSvc.getUserModel().Location ? userSvc.getUserModel().Location.Id : ''
      };

      // devices filters array that is used in the template
      $scope.devicesFilters = [];

      // accessories filters array that is used in the template
      $scope.accessoriesFilters = [];
      $scope.teams = [];

      /**
       * Executes a request for loading data for devices/accessories filters
       */
      var _loadFilters = function(type) {
        restSvc.get('filters', type).then(function(result) {

          // check for errors
          if (result.Message != "OK") {
            console.log('Error in loading the devices filters!');
          }

          // remove the OK message because it's treated as a filter in the loop for populating the devices filters
          delete result.Message;

          // expose devices filters to the global variable so they can be accessed from other functions
          if (type == 'devices') {
            deviceFiltersObject = result;
          }

          // iterate all filters to populate the devices filters object that will be passed to the template
          for (var key in result) {
            var scopeKey = type + 'Filters';
            var filterObject = {
              label: configSvc.getFilterLabelValue(key),
              param: configSvc.getFilterParamValue(key),
              selectedValue: 'All',
              values: result[key],
              disabledClass: ''
            };

            // for the city get the selected value from the user's location
            // by default this filter is selected based on the user's location
            if (key == 'Cities') {
              filterObject.selectedValue = userSvc.getUserModel().Location ? userSvc.getUserModel().Location.Name : 'All';
            }

            $scope[scopeKey].push(filterObject);

            // right after the OS filter add two more filters - for the manufacturers and models that are disable initially
            if (key == 'Platforms' && type == 'devices') {
              $scope[scopeKey].push({
                label: configSvc.getFilterLabelValue('Manufacturers'),
                values: [],
                disabledClass: 'disabled',
                param: configSvc.getFilterParamValue('Manufacturers'),
                selectedValue: 'All'
              });

              $scope[scopeKey].push({
                label: configSvc.getFilterLabelValue('Models'),
                values: [],
                disabledClass: 'disabled',
                param: configSvc.getFilterParamValue('Models'),
                selectedValue: 'All'
              });
            }
          }

        }, function(err) {
          console.log('Error', err);
        });
      };

      $scope.stopPropagation = function(e) {
        e.stopPropagation();
      };

      /**
       * Reset all filters
       */
      $scope.clearFilters = function() {
        // in the criteria object we need only this basic properties
        // all others should be removed
        $scope.criteria = {
          offset: 0,
          limit: 20,
          SortColumns: 'DateTaken.DESC',
          Keyword: '',

          // by default filter devices/accessories based on user's location
          TownId: userSvc.getUserModel().Location ? userSvc.getUserModel().Location.Id : ''
        };

        // clear the selected values from the devices and accessories filters objects
        $scope.devicesFilters.forEach(function(filter) {

          // town filter by default is selected (user location)
          // so get the value from the user location
          if (filter.param == "TownId") {
            filter.selectedValue = userSvc.getUserModel().Location ? userSvc.getUserModel().Location.Name : 'All';
          } else {
            filter.selectedValue = "All";
          }

        });

        $scope.accessoriesFilters.forEach(function(filter) {

          // town filter by default is selected (user location)
          // so get the value from the user location
          if (filter.param == "TownId") {
            filter.selectedValue = userSvc.getUserModel().Location ? userSvc.getUserModel().Location.Name : 'All';
          } else {
            filter.selectedValue = "All";
          }
        });

        var showAllButton = angular.element(document.getElementsByClassName('show-all'));
        var availabilityButton = angular.element(document.getElementsByClassName('nav-availability')).children();
        var sortDirectionButton = angular.element(document.getElementById('sort-direction'));

        // mark as active show all button
        availabilityButton.removeClass('active');
        showAllButton.addClass('active');

        // by default sort direction is desc
        sortDirectionButton.removeClass('desc');
        sortDirectionButton.addClass('asc')

        // when the filters are cleared load again accessories or devices
        $scope.$broadcast('loadItems', true);
      };

      /**
       * Switch between devices and accessories filters
       * @param {Object} event
       */
      $scope.toggleFilterTab = function(event) {
        var selectedTab, selectedTabId, itemsType;

        // if a tab is clicked
        if (typeof event != 'undefined') {
          // get the selected tab element
          selectedTab = angular.element(event.currentTarget);

          // get the id of the selected tab
          selectedTabId = selectedTab.attr('id');

          itemsType = selectedTabId.replace('-tab', '');
        }
        // otherwise select the appropriate tab depending on the url
        else {

          // get the items type (devices or accessories) from the location
          itemsType = $location.path().replace('/search/', '');

          selectedTabId = itemsType + '-tab';
          selectedTab = angular.element(document.getElementById(selectedTabId));
        }

        // find the corresponding content of the selected tab
        var selectedTabContent = angular.element(document.getElementById(selectedTabId.replace('tab', 'filters')));

        // get all tab elements
        var tabs = angular.element(document.getElementsByClassName('tab'));

        // get all tab content elements
        var tabContents = angular.element(document.getElementsByClassName('tab-content'));

        // unselect all tabs
        tabs.removeClass('selected');

        // mark as selected the clicked one
        selectedTab.addClass('selected');

        // hide all filters
        tabContents.addClass('ng-hide');

        // show the filter for the selected tab
        selectedTabContent.removeClass('ng-hide');

        // if accessories or devices filters are not loaded yet
        if ((!$scope.accessoriesFilters.length && itemsType == 'accessories') || (!$scope.devicesFilters.length && itemsType == 'devices')) {
          _loadFilters(itemsType);
        }

        // clear the selected filters because the object criteria is shared between devices and accessories controllers
        $scope.clearFilters();
      };

      // select a filter tab and load items depending on the url
      $scope.toggleFilterTab();

      /**
       * Returns an element from the filters object based on the passed param.
       * @return {Object}
       */
      var _findFiltersObject = function(array, param) {
        for (var i = 0; i < array.length; i++) {
          if (param == array[i].param) {
            return array[i];
          }
        }
      };

      /**
       * Set the selected device filter in the criteria object sent to the server.
       */
      $scope.selectDeviceFilter = function(event) {
        // get the selected filter element
        var selectedFilter = angular.element(event.currentTarget);

        // get the key and the value of the selected filter
        var selectedFilterKey = selectedFilter.parent().attr('id');
        var selectedFilterValue = selectedFilter.attr('id');

        // show the selected value from the dropdown
        var deviceFilterObject = _findFiltersObject($scope.devicesFilters, selectedFilterKey);
        deviceFilterObject.selectedValue = selectedFilter.html();

        // set the selected filter in the criteria object that is sent to the server
        // if option 'all' is selected then no filter is passed to the server
        if (selectedFilterValue == 'all') {
          $scope.criteria[selectedFilterKey] = undefined;
        } else {
          $scope.criteria[selectedFilterKey] = selectedFilterValue;
        }

        // when a filter is changed we want to load the first page again
        $scope.criteria.offset = 0;

        var _clearFilter = function(criteriaId) {
          // disable the filter
          var filterObject = _findFiltersObject($scope.devicesFilters, criteriaId);
          filterObject.selectedValue = 'All';
          filterObject.disabledClass = 'disabled';
          filterObject.values = [];
          $scope.criteria[criteriaId] = undefined;
        };

        // when an OS is selected enable and populate the manufacturer filter
        if (selectedFilterKey == 'OsId') {

          // first reset the manufacturer and model filters if such
          _clearFilter('ManufacturerId');
          _clearFilter('ModelId');

          // get the list of all OSs
          var platforms = deviceFiltersObject.Platforms;
          for (var i = 0; i < platforms.length; i++) {

            // find the selected OS
            if (selectedFilterValue == platforms[i].Id) {

              // populate and enable the manufacturers filter
              var manufacturerFilter = _findFiltersObject($scope.devicesFilters, 'ManufacturerId');
              manufacturerFilter.values = platforms[i].Manufacturers;
              manufacturerFilter.disabledClass = '';
              break;
            }
          }
        }

        // when a manufacturer is selected enable and populate the models filter
        if (selectedFilterKey == 'ManufacturerId') {

          // first reset the model filter if such
          _clearFilter('ModelId');

          // get the list of all manufacturers
          var manufacturers = _findFiltersObject($scope.devicesFilters, 'ManufacturerId').values;

          for (var i = 0; i < manufacturers.length; i++) {

            // find the selected manufacturer
            if (selectedFilterValue == manufacturers[i].Id) {
              // populate and enable the models filter
              var modelFilter = _findFiltersObject($scope.devicesFilters, 'ModelId');
              modelFilter.values = manufacturers[i].Models;
              modelFilter.disabledClass = '';
              break;
            }
          }
        }

        // load devices according to the selected filters
        $scope.$broadcast('loadItems', false);
      }

      /**
       * Set the selected accessory filter in the criteria object sent to the server.
       */
      $scope.selectAccessoryFilter = function(event) {
        // get the selected filter element
        var selectedFilter = angular.element(event.currentTarget);

        // get the key and the value of the selected filter
        var selectedFilterKey = selectedFilter.parent().attr('id');
        var selectedFilterValue = selectedFilter.attr('id');

        // show the selected value from the dropdown
        var deviceFilterObject = _findFiltersObject($scope.accessoriesFilters, selectedFilterKey);
        deviceFilterObject.selectedValue = selectedFilter.html();

        // set the selected filter in the criteria object that is sent to the server
        // if option 'all' is selected then no filter is passed to the server
        if (selectedFilterValue == 'all') {
          $scope.criteria[selectedFilterKey] = undefined;
        } else {
          $scope.criteria[selectedFilterKey] = selectedFilterValue;
        }

        // when a filter is changed we want to load the first page again
        $scope.criteria.offset = 0;

        // load accessories according to the selected filters
        $scope.$broadcast('loadItems', false);
      }

      /**
       * Filters available/not available devices.
       * This function is wrapped in both child controllers - for devices and accessories.
       * @param {Object} event
       */
      $scope.filterByAvailability = function(event) {
        // get the selected filter element
        var selectedFilter = angular.element(event.currentTarget);

        // get all siblings
        var siblings = selectedFilter.parent().children();

        var availabilityId = selectedFilter.attr('id');

        // first mark as inactive all elements
        siblings.removeClass('active');

        // mark as active the selected availability filter
        selectedFilter.addClass('active');

        // set the selected availability filter in the criteria object
        $scope.criteria.IsAvailable = availabilityId;

        // when a filter is changed we want to load the first page again
        $scope.criteria.offset = 0;
      };

      restSvc
        .get('teams', 'list')
        .then(function(teams) {
          teams = teams.Teams;

          teams.forEach(function(singleTeam) {
            $scope.teams.push(new teamModel(singleTeam));
          });
        });

      /**
       * Returns the id of the 'Other' team
       * @return {String}
       */
      $scope.getDefaultTeamId = function() {
        for (var i = 0; i < $scope.teams.length; i++) {
          if ($scope.teams[i].Name == 'Other') {
            return $scope.teams[i].Id;
          }
        }
        return null;
      }
    }]);
})();
