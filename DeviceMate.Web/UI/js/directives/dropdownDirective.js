// Dropdown List directive

'use strict';

angular
  .module('devicemateDropdown', [
    'ui.bootstrap.dropdown'
  ])
  .directive('devicemateDropdown', ['$timeout',
    function($timeout) {
      return {
        restrict: 'A',

        // Replace the current element with the contents of the HTML template file
        replace: true,
        /*
            Isolated/local scope
            --------------------------
            dropdownData - populate dropdown list from a javascript array, if no url is given
            parser - pass in a function that directive can invoke to parse the data returned by Ajax request
            url - the URL to make the request to
            clickhandler - pass in a custom click handler

            Dynamic/Static data format
            -----------------------------------
            [
             {
                "text":"TEXT",
                "value":"VALUE",
                "options":{
                   "ATTRIBUTE_NAME":"ATTRIBUTE_VALUE",
                   "ATTRIBUTE_NAME":"ATTRIBUTE_VALUE",
                  ...
                }
             },
             {
                "text":"TEXT",
                "value":"VALUE"
             },
              ...
            ]
        */
        scope: {
          class: '@',
          clickhandler: '&',
          responsehandler: '&',
          placeholder: '@',
          optionsClass: '@',
          parser: '&',
          actionUrl: '@',
          method: '@',
          getUrl: '@',
          data: '='
        },

        // HTML template file
        templateUrl: '/UI/templates/dropdown_list.html',
        // Dropdown controller
        controller: function($scope, $element, $timeout, $attrs, $filter, $parse) {
          var timeoutPromise = null;
          $element.on('mouseover', function() {
            if(timeoutPromise !== null) {
              $timeout.cancel(timeoutPromise);
            }
            $element.addClass('open');
          });

          $element.on('mouseout', function() {
            timeoutPromise = $timeout(function() {
              $element.removeClass('open');
            }, 500);
          });

          if ($attrs.data) {
            if ($scope.$eval($attrs.data)) {
              $scope.dropdownData = $parse($attrs.data)($scope.$parent);
            } else {

              //assign data at first
              $scope.dropdownData = $scope.data;
              $scope.isopen = false;

              //watched, because otherwise changes may not be applied
              $scope.$watchCollection('data', function() {
                $scope.dropdownData = $scope.data;
              });
            }

            //get the selected value
            var selectedValue = $filter('filter')($scope.dropdownData, {
              'selected': true
            }, true)[0];

            //check if another selected value available

            // if there is a selected value and it is the default selected one,
            if (($scope.dropdownData.indexOf(selectedValue) !== -1)) {
              selectedValue.selected = true;
              $scope.currentValue = selectedValue;
            } else {
              $scope.currentValue = {
                text: ($scope.placeholder || 'select'),
                value: 'nA'
              };
            }

          } else {
            // Requesting remote data
            if ($scope.getUrl) {

              // Ajax.get(
              //   $scope.getUrl,
              //   $scope.parameters, {},
              //   function() {
              //   },
              //   function() {
              //   },
              //   function(res) {
              //     if ($scope.parser) {
              //       // Use parser function if available
              //       $scope.dropdownData = $scope.parser({
              //         data: res.data
              //       });

              //     } else {
              //       $scope.dropdownData = res.data;
              //     }
              //   },
              //   function() {});

              // Get data from javascript array /see 'dropdownData' property of the local scope/
            }

          }

          // Click handler
          $scope.handleClick = function(item) {
            //get the selected value
            var selectedValue = $filter('filter')($scope.dropdownData, {
              'selected': true
            }, true)[0];

            //if selected value available, make it unselected and select the new one
            if (selectedValue) {
              selectedValue.selected = false;
            }
            item.selected = true;
            //assign the current item to be the selected item

            //POSSIBLE TODO
            //this can be avoided by simply using
            //a filter on the datasource for selected:true in the dropdownlist template
            $scope.currentValue = item;

            //close dropdown
            $scope.isopen = false;


            // Execute Ajax request if url and method
            if ($scope.actionUrl && $scope.method) {

              // Ajax[$scope.method](
              //   $scope.actionUrl,
              //   $scope.parameters, {},
              //   function() {},
              //   function() {},
              //   function(res) {
              //     // Response handler (optional)
              //     if ($attrs.responsehandler) {
              //       $scope.responsehandler({
              //         data: res.data
              //       });
              //     }
              //   },
              //   function() {});
            }
            // Custom click handler (optional)
            if ($attrs.clickhandler) {
              $scope.clickhandler({
                data: item
              });
              if ($attrs.donotpropagate) {
                event.stopPropagation();
              }
              return;
            }
            if ($attrs.donotpropagate) {
              event.stopPropagation();
            }
          };
        }
      };
    }
  ]);
