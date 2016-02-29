(function() {
  'use strict';

  DeviceMateApp
    .factory('configSvc', [function() {
      //
      // CONFIGS:
      //

      // mapping object for the hex values of the colors
      var colorHexMap = {
        White: '#FFF',
        Black: '#000',
        Blue: '#367EB3',
        Gray: '#67676C',
        Orange: '#FBAC42',
        Pink: '#FFC0CB',
        'Space Gray': '#DCDCDC',
        Gold: '#DEB887',
        'White Frost': '#FAFAFF',
        'Titanium Bronze': '#98817B'
      };

      // mapping object for the filters labels
      var filterLabelsMap = {
        Cities: 'City',
        Colors: 'Color',
        Platforms: 'OS',
        Manufacturers: 'Manufacturer',
        Models: 'Model',
        ScreenSize: 'Screen Size',
        ScreenHeight: 'Height (px)',
        ScreenWidth: 'Width (px)',
        Teams: 'Team',
        Descriptions: 'Description',
        DeviceType: 'Type',
        AccessoryType: 'Type'
      };

      // mapping object for the filter keys that are used in the criteria object
      var filterParamsMap = {
        Cities: 'TownId',
        Colors: 'ColorId',
        Platforms: 'OsId',
        Manufacturers: 'ManufacturerId',
        Models: 'ModelId',
        ScreenSize: 'ScreenSizeId',
        ScreenHeight: 'ResolutionHeightId',
        ScreenWidth: 'ResolutionWidthId',
        Teams: 'TeamId',
        Descriptions: 'DescriptionId',
        DeviceType: 'TypeId',
        AccessoryType: 'TypeId'
      };

      var deviceSortColumns = [
        { id: "DateTaken", name: "Date taken" },
        { id: "Model", name: "Model" },
        { id: "Name", name: "Name" },
        { id: "Type", name: "Type" },
        { id: "Town", name: "City" },
        { id: "Os", name: "OS" },
        { id: "OsVersion", name: "OS version" },
        { id: "Team", name: "Team" },
        { id: "Color", name: "Color" },
        { id: "ScreenSize", name: "Screen Size" },
        { id: "ResolutionWidth", name: "Resolution Width" },
        { id: "ResolutionHeight", name: "Resolution Height" },
        { id: "Available", name: "Available" },
        { id: "Email ", name: "Holder" },
        { id: "Info", name: "Aditional Info" }
      ];

      var accessorySortColumns = [
        { id: "DateTaken", name: "Date taken" },
        { id: "Type", name: "Type" },
        { id: "Town", name: "City" },
        { id: "Os", name: "OS" },
        { id: "Team", name: "Team" },
        { id: "Color", name: "Color" },
        { id: "Available", name: "Available" },
        { id: "Description", name: "Description" },
        { id: "Email ", name: "Holder" },
        { id: "Info", name: "Aditional Info" }
      ];

      //
      // METHODS:
      //

      /**
       * Return the hex value of the passed color
       * @param  {String} color
       * @return {String}
       */
      var getHexValue = function(color) {
        return colorHexMap[color];
      };

      /**
       * Returns the list of all colors
       * @returns {Object}
       */
      var getColorHexMap = function() {
        return colorHexMap;
      };

      /**
       * Returns the list of labels for filters in the search device/accessory pages
       * @returns {Object}
       */
      var getFilterLabelsMap = function() {
        return filterLabelsMap;
      };

      /**
       * Return the label for the specified key
       * @param  {String} key
       * @return {String}
       */
      var getFilterLabelValue = function(key) {
        return filterLabelsMap[key];
      };

      /**
       * Returns the list of params for filters that are sent in the server request
       * @returns {Object}
       */
      var getFilterParamsMap = function() {
        return filterParamsMap;
      };

      /**
       * Return the id param for the specified key
       * @param  {String} key
       * @return {String}
       */
      var getFilterParamValue = function(key) {
        return filterParamsMap[key];
      };

      /**
       * Returns the sort columns vavilable for devices
       * @returns {Object}
       */
      var getDeviceSortColumns = function() {
        return deviceSortColumns;
      };

      /**
       * Returns the sort columns vavilable for accessories
       * @returns {Object}
       */
      var getAccessorySortColumns = function() {
        return accessorySortColumns;
      };

      return {
        getHexValue: getHexValue,
        getColorHexMap: getColorHexMap,
        getFilterLabelsMap: getFilterLabelsMap,
        getFilterLabelValue: getFilterLabelValue,
        getFilterParamsMap: getFilterParamsMap,
        getFilterParamValue: getFilterParamValue,
        getDeviceSortColumns: getDeviceSortColumns,
        getAccessorySortColumns: getAccessorySortColumns
      };
    }]);
})();
