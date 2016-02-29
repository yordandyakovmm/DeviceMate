(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .controller('ModalNewFilterCtrl', ModalNewFilterCtrl);

  ModalNewFilterCtrl.$inject = ['$scope'];

  function ModalNewFilterCtrl($scope) {
    var vm = this;

    vm.filter = '';
    vm.title = angular.isDefined($scope.ngDialogData)
      ? $scope.ngDialogData.title
      : '';
  }
})();

