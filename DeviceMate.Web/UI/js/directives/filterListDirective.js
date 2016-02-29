(function() {
  'use strict';

  angular
    .module('DeviceMateApp')
    .directive('filterList', filterList);

  function filterList() {
    var directive = {
      restrict: 'EA',
      transclude: true,
      templateUrl: '/UI/templates/filter_list.html',
      link: FilterListLink,
      controller: FilterListController,
      controllerAs: 'vm',
      scope: {
        activeItem: '=',
        filter: '=',
        onFilterChange: '&'
      },
      bindToController: true
    };

    return directive;
  }

  /**
   * The `filterList` renders a list of filters. The only required param is the filter itself.
   * For example to display all Manufacturers an array of objects should be provided
   * containing the manufacturer id and name to display.
   *
   * Optionally can be provided a activeItem model to be updated when a user selects
   * a filter or you can update it manually with onFilterChange callback.
   *
   * ngTransclude is used to provide a default content (i.e maybe when there are no available filters).
   *
   * @param {string} listIconClass The font-icon class to use for every item.
   * Default is `glyphicon glyphicon-menu-right`.
   *
   * @param {object=} activeItem The ngModel to update when a user selects a filter.
   *
   * @param {array=} filter The ngModel filter to display.
   *
   * @param {function&} onFilterChange Callback function to be invoked when a user
   * selects a filter. Accepts an `item` argument with the updated item.
   *
   * @example
   * <pre>
   * <filter-list list-icon-class="dm-glyph-os nav-glyph"
   *                         active-item="device.Manufacturer"
   *                         filter="filters.Manufacturers"
   *                         on-filter-change="manufacturerChanged(item)">
   *   <em ng-show="!filters.Manufacturers.length">
   *     No filters to show. Select a different manufacturer/os
   *   </em>
   * </filter-list>
   * </pre>
   */
  FilterListController.$inject = ['$scope'];
  function FilterListController($scope) {
    var vm = this;

    vm.activeItemId = null;
    vm.activateItem = activateItem;

    /** @todo maybe update vm.activeItemId directly instead of a watch */
    $scope.$watch('vm.activeItem', function(newActiveItem) {
      if (newActiveItem && angular.isDefined(newActiveItem.Id)) {
        vm.activeItemId = newActiveItem.Id;
      }
    });

    function activateItem(item) {
      vm.activeItemId = item.Id;

      // Update the shared scope
      vm.activeItem = item;
      vm.onFilterChange({item: item});
    }
  }

  function FilterListLink(scope, $element, attrs) {
    var listIcon = attrs.listIconClass || 'glyphicon glyphicon-menu-right';
    scope.directiveElement = $element; // keep reference to the directive element for the $watch
    scope.vm.getListIcon = getListIcon;

    //* Scroll to active element only the first time */
    var unbindWatcher = scope.$watch('vm.activeItemId', function(activeItemId) {

      //* Limit scrolling only when activeItemId is actually available */
      if (activeItemId) {
        scrollToActiveElement(activeItemId);
        unbindWatcher();
      }
    });

    function scrollToActiveElement(activeItemId) {
      var ul = scope.directiveElement.find('ul')[0],
          listItems = ul.children,
          borderWidth = 2; // include border width in scrolling

      angular.forEach(listItems, function(item){
        if (parseInt(item.dataset.id) === activeItemId) {
          ul.scrollTop = item.offsetTop - item.offsetHeight - borderWidth;
        }
      });
    }

    function getListIcon(item) {
      return scope.vm.activeItemId === item.Id ? 'glyphicon glyphicon-ok' : listIcon;
    }
  }
})();
