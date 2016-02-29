(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .factory('modalSvc', modalSvc);

  modalSvc.$inject = ['ngDialog'];

  /**
   * @ngdoc object
   * @name modalSvc
   *
   * @requires ngDialog
   *
   * @todo Add support for the full ngDialog API.
   *
   * @description
   * Service/Factory. A wrapper services for ngDialog that manages all system modals/dialogs.
   */
  function modalSvc(ngDialog) {
    var currentDialog;
    var service = {
      alert: alert,
      confirm: confirm,
      closeAll: closeAll,

      openNewFilterDialog: openNewFilterDialog
    };

    return service;

    /**
     * Opens a customizable dialog. Accepts an options objects to create the dialog.
     *
     * @link https://github.com/likeastore/ngDialog#openoptions
     *
     * @param  {object} data See @link for all available options
     * @return {object}      Contains useful (in a somewhat unconsisten) properties.
     * See https://github.com/likeastore/ngDialog#returns for the official API.
     */
    function alert(data) {
      var modalDefaults = {
        template: '/UI/templates/modal.html',
        controller: 'ModalCtrl',
      };
      var options = angular.extend(modalDefaults, data);

      return ngDialog.open(options);
    }

    /**
     * Opens the default confirm dialog. Accepts an argument for
     * the title and dialog body message to be passed to the dialog controller.
     *
     * @link https://github.com/likeastore/ngDialog#data-string--object--array
     *
     * @param  {Any} data Any serializable data that you want to be stored in the controller's dialog scope
     * @return {promise}  A promise that is either resolved or rejected depending on the way the dialog was closed.
     */
    function confirm(data) {
      return ngDialog.openConfirm({
        template: '/UI/templates/modal.html',
        controller: 'ModalCtrl',
        data: data
      });
    }

    /**
     * Closes all active dialogs.
     *
     * @link https://github.com/likeastore/ngDialog#closeallvalue
     *
     * @param  {Any} value Optional value to resolve all of dialog promises
     */
    function closeAll(value) {
      ngDialog.closeAll(value);
    }

    /**
     * Opens a custom 'Add New Filter' dialog.
     *
     * @param  {Any} data Any serializable data that you want to be stored in the controller's dialog scope.
     * The title of the dialog can be changed by passing data.title
     *
     * @return {promise}      [description]
     */
    function openNewFilterDialog(data) {
      return ngDialog.open({
        template: '/UI/templates/modal_new_filter.html',
        controller: 'ModalNewFilterCtrl',
        controllerAs: 'vm',
        data: data
      });
    }
  }

})();
