(function () {
    var DeviceMateApp = angular.module('DeviceMateApp', ['ui.router']);

    DeviceMateApp
            .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider',
                function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider) {

                    // Set default route
                    // $urlRouterProvider.otherwise("/home");

                    // Routing table:
                    $stateProvider
                            .state('home', {
                                url: "/home",
                                templateUrl: 'templates/home.html',
                                controller: 'HomeCtrl'
                            })
                            .state('devices', {
                                url: "/devices",
                                templateUrl: 'templates/grid_devices.html',
                                controller: 'HomeCtrl'
                            })
                            .state('accessories', {
                                url: "/accessories",
                                templateUrl: 'templates/grid_accessories.html',
                                controller: 'HomeCtrl'
                            })
                            .state('filters', {
                                url: "/filters",
                                templateUrl: 'templates/filters.html',
                                controller: 'HomeCtrl'
                            });
                }
            ])
            .controller('RootCtrl', function ($scope) {
                $scope.root = {
                    // flags that control the visibility of the dropdowns in the header
                    showManageNav: false,
                    showUserNav: false,
                    /**
                     * Show or hide the manage dropdown in the header
                     */
                    toggleManageNav: function () {
                        this.showManageNav = !this.showManageNav;
                    },
                    /**
                     * Show or hide the dropdown for logging out in the header
                     */
                    toggleUserNav: function () {
                        this.showUserNav = !this.showUserNav;
                    },
                    /**
                     * Expand the clicked filter
                     * @param {Object} event
                     */
                    toggleFilterNav: function (event) {
                        // get the clicked button element
                        var clickedButton = angular.element(event.currentTarget);

                        // do nothing if the filter is disabled
                        if (clickedButton.hasClass('disabled')) {
                            return;
                        }

                        // get the collapsible part of the clicked element
                        var currentDropdownContent = angular.element(event.currentTarget.nextElementSibling);

                        // get the collapsible parts of all filters
                        var dropdownContents = angular.element(document.getElementsByClassName('filter-content'));

                        // clicked filter is expanded
                        // then just collapse it
                        if (!currentDropdownContent.hasClass('ng-hide')) {
                            currentDropdownContent.addClass('ng-hide');
                        }
                        // clicked filter collapsed
                        else {
                            // first collapse the other filter
                            dropdownContents.addClass('ng-hide');

                            // expand the selected filter
                            currentDropdownContent.removeClass('ng-hide');
                        }
                    },
                    /**
                     * Switch between devices and accessories filters
                     * @param {type} event
                     */
                    toggleFilterTab: function(event){
                        // get the selected tab element
                        var selectedTab = angular.element(event.currentTarget);
                        
                        // get the id of the selected tab
                        var selectedTabId = selectedTab.attr('id');
                        
                        // find the corresponding content of the selected tab
                        var selectedTabContent = angular.element(document.getElementById(selectedTabId.replace('tab', 'filters')));
                        
                        // get all tab elements
                        var tabs = angular.element(document.getElementsByClassName('tab'));
                        
                        // get all tab content elements
                        var tabContents = angular.element(document.getElementsByClassName('tab-content'));
                        
                        // unselect all tabs
                        tabs.removeClass('selected');
                        
                        // mark as selected the clicked one
                        selectedTab.addClass('selected');
                        
                        // hide all filters
                        tabContents.addClass('ng-hide');
                        
                        // show the filter for the selected tab
                        selectedTabContent.removeClass('ng-hide');
                    }
                };
            })
            .controller('HomeCtrl', function ($scope) {

            });

    window.DeviceMateApp = DeviceMateApp;
})();