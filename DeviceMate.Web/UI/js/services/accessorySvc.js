(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .factory('accessorySvc', accessorySvc);

  accessorySvc.$inject = ['$q', 'restSvc'];

  function accessorySvc($q, restSvc) {
    var service = {
      getAccessory: getAccessory,
      saveAccessory: saveAccessory
    };

    return service;

    /**
     * Gets information about an accessory by the provided id.
     *
     * @param  {number} id The accessory id we're interested in.
     * @return {promise}
     */
    function getAccessory(id) {
      return restSvc.get('accessories', 'show', { id: id });
    }

    /**
     * Performs a validation check against the provided accessory.
     *
     * If it's a valid accessory will send a POST request to the REST API else - will reject with a message.
     *
     * @param  {object} accessory The accessory to be saved
     * @return {promise}
     */
    function saveAccessory(accessory) {
      var deferred = $q.defer();

      if (_validateAccessory(accessory)) {
        return restSvc.post('accessories', 'add', accessory);
      } else {
        deferred.reject({ reason: 'The accessory seems to be invalid' });

        return deferred.promise;
      }
    }

    /**
     * Check if the accessory we're saving has the required params.
     *
     * 'Holder' property is not validated, because it is resolved in the $state.
     * 'Name' and 'Info' properties have required attr in the form.
     *
     * @todo  Could be done with a proper model and skip entirely the validation.
     *
     * @param  {object} accessory The accessory to be checked
     * @return {bool}
     */
    function _validateAccessory(accessory) {
      var isValid = true;

      if (!accessory.Description || !accessory.Description.Id)
        isValid = false;

      if (!accessory.Type || !accessory.Type.Id)
        isValid = false;

      return isValid;
    }
  }
})();
