(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .controller('ModalCtrl', ModalCtrl);

  ModalCtrl.$inject = ['$scope', '$sce'];

  function ModalCtrl($scope, $sce) {
    $scope.modal = {};

    $scope.modal.title = angular.isDefined($scope.ngDialogData)
      ? $sce.trustAsHtml($scope.ngDialogData.title)
      : 'Confirm action';

    $scope.modal.message = angular.isDefined($scope.ngDialogData)
      ? $sce.trustAsHtml($scope.ngDialogData.message)
      : '';
  }
})();

