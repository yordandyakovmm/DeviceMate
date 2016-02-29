(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .factory('filterSvc', filterSvc);

  filterSvc.$inject = ['$q', 'restSvc', 'modalSvc', 'FILTERS'];
  function filterSvc($q, restSvc, modalSvc, FILTERS) {
    var service = {
      getDevices: getDevices,
      getAllDevices: getAllDevices,
      getAccessories: getAccessories,
      getAllAccessories: getAllAccessories,

      saveFilter: saveFilter,
      deleteFilter: deleteFilter
    };

    return service;

    /**
     * List available filters for devices. Useful in search.
     *
     * @return {promise}
     */
    function getDevices() {
      return restSvc.get('filters', 'devices');
    }

    /**
     * List all filters for devices. Useful when creating or editing device.
     *
     * @return {promise}
     */
    function getAllDevices() {
      return restSvc.get('filters', 'devices/all');
    }

    /**
     * List available filters for accessories. Useful in search.
     *
     * @return {promise}
     */
    function getAccessories() {
      return restSvc.get('filters', 'accessories');
    }

    /**
     * List all filters for devices. Useful when creating or editing accessory.
     *
     * @return {promise}
     */
    function getAllAccessories() {
      return restSvc.get('filters', 'accessories/all');
    }

    /**
     * A procedure to save a new filter. Returns a promise to be resolved. All params are required
     * and are expected to be valid. Otherwise the promise will be rejected.
     *
     * @param  {string} filterType Property name from the FILTERS dictionary. Is used to form the URL
     * for the REST API endpoint.
     *
     * @param  {string} filterName The new filter name. Send as POST param to the REST API endpoint.
     *
     * @return {promise}
     */
    function saveFilter(filterType, filterName) {
      var deferred = $q.defer();

      /** Check if there is such a filter */
      if (angular.isDefined(FILTERS[filterType])) {

        /** Open a confirm dialog */
        return modalSvc.openNewFilterDialog()
          .closePromise
          .then(function(data) {

            /** If there is a new filter we'll proceed making the REST call to save it */
            if (data.value.newFilter) {
              return restSvc.post(FILTERS[filterType], 'save', { Name: data.value.newFilter });
            } else {
              deferred.reject({ reason: 'User decided that he doesn\'t need a new filter' });

              return deferred.promise;
            }
          });
      } else {
        deferred.reject({ reason: 'No such filterType' });

        return deferred.promise;
      }
    }

    /**
     * A procedure to delete a filter. Returns a promise to be resolved. All params are required
     * and are expected to be valid. Otherwise the promise will be rejected.
     *
     * @param  {string} filterType Property name from the FILTERS dictionary. Is used to form the URL
     * for the REST API endpoint.
     *
     * @param  {string} filterName The filter id. Also used as part of the URL.
     *
     * @return {promise}
     */
    function deleteFilter(filterType, filterId) {
      var deferred = $q.defer(),
          modalOptions = {
            message: 'Are you sure you want to delete this filter?'
          };

      /** Check if there is such a filter */
      if (angular.isDefined(FILTERS[filterType])) {

        /** Open a confirm dialog */
        return modalSvc.confirm(modalOptions)
          .then(function() {
            return restSvc.get(FILTERS[filterType], 'delete/' + filterId);
          })
          .catch(function() {
            deferred.reject({ reason: 'User decided that he doesn\'t want to delete this filter' });

            return deferred.promise;
          });
      } else {
        deferred.reject({ reason: 'No such filterType' });

        return deferred.promise;
      }
    }
  }
})();
