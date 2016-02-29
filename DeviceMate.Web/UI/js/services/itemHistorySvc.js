(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .factory('itemHistorySvc', itemHistorySvc);

  itemHistorySvc.$inject = ['restSvc'];

  function itemHistorySvc(restSvc) {
    var service = {
      getHistory: getHistory
    };

    return service;

    function getHistory(itemType, itemId) {
      return restSvc.get(itemType, 'history', { "id": itemId });
    }
  }

})();

