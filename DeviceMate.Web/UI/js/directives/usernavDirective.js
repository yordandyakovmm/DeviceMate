(function() {
  'use strict';

  DeviceMateApp
    .directive('devicemateUserNav', ['$timeout', function($timeout) {
      return {
        restrict: 'A',
        link: function(scope, $element, attrs) {
          var timeoutPromise = null;

          $element.on('mouseover', function() {
            if (timeoutPromise !== null) {
              $timeout.cancel(timeoutPromise);
            }
            $element.addClass('open');
          });

          $element.on('mouseout', function() {
            timeoutPromise = $timeout(function() {
              $element.removeClass('open');
            }, 500);
          });
        }
      };
    }]);
})();
