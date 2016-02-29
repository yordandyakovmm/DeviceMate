(function() {
  'use strict';

  var FILTERS = {
    ACCESSORY_DESCRIPTION: 'accessorydescriptions',
    ACCESSORY_TYPE: 'accessorytypes',
    COLORS: 'colors',
    OS: 'os'
  };

  angular
    .module('DeviceMateApp')
    .constant('FILTERS', FILTERS);
})();
