(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .factory('FilterModel', FilterModel);

  function FilterModel() {
    return FilterModel;

    function FilterModel() {
      this.AccessoryType = [],
      this.Cities = [],
      this.Colors = [],
      this.Descriptions = [],
      this.Platforms = [],
      this.Teams = []
    }
  }

})();

