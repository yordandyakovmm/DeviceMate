(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .directive('itemHistory', itemHistory);

  function itemHistory($document) {
    var directive = {
      restrict: 'EA',
      templateUrl: '/UI/templates/item_history.html',
      controller: ItemHistoryController,
      controllerAs: 'vm',
      scope: {
        item: '=',
        itemType: '@',
        onHistoryClose: '&'
      },
      bindToController: true
    };

    return directive;
  }

  /**
   * The `itemHistory` directive displays the usage history of an item (device or accessory)
   * in a togglable window.
   *
   * @param {object@} item The item which history is required. From this object only
   * the property itemID is required to get the history via xhr.
   *
   * @param {string@} itemType The item type to query for.
   *
   * @param {function&} onHistoryClose Callback function to be invoked when the
   * window should close.
   *
   * @example
   * <pre>
   * <item-history item="currentDevice"
   *               item-type="devices"
   *               on-history-close="hideHistory()"
   *               ng-show="historyShown">
   * </item-history>
   * </pre>
   */
  ItemHistoryController.$inject = ['$scope', 'itemHistorySvc'];
  function ItemHistoryController($scope, itemHistorySvc) {
    var vm = this;

    vm.itemHistory = [];
    vm.hideHistory = hideHistory;
    vm.isHistoryEmpty = false;

    $scope.$watch('vm.item', onItemSelect);

    function onItemSelect() {
      if (vm.item) {
        vm.itemHistory = [];

        itemHistorySvc.getHistory(vm.itemType, vm.item.itemID)
          .then(onItemHistorySuccess);
      }
    }

    function onItemHistorySuccess(response) {
      vm.itemHistory = response.History;

      if (!response.History.length) {
        vm.isHistoryEmpty = true;
      }
    }

    function hideHistory() {
      vm.onHistoryClose();
    }
  }
})();

