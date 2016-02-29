$(document).ready(function () {
    ListLoader.init("#SearchFilter_OsId", "#SearchFilter_ManufacturerId", "#SearchFilter_ModelId", "#SearchFilter_TownID");
    SearchPresenter.init();
});

var SearchPresenter = {

    searchFilter: {},

    clearFilter: function () {
        $("div.well input[type=text]").val("");
        $("div.well select").val([]);
        $("#SearchFilter_ManufacturerId").find("option").remove();
        $("#SearchFilter_ModelId").find("option").remove();
        $("#SearchFilter_TownID").selectedIndex = 0;
        this.searchFilter = {};
    },

    init: function () {
        var self = this;

        $("#btnClear").click(function () {
            SearchPresenter.clearFilter();
        });

        $(".btn.delete").click(function (event) {
            if (!confirm("Are you sure you want to delete the device?")) {
                event.preventDefault();
            }
            else {
                var url = $(this).data("url");
                var itemId = $(this).data("itemid");
                var xhr, params;
                params = {
                    id: itemId
                }

                xhr = $.ajax({ url: url, data: params, type: "POST" });
                xhr.success(function (result) {
                    if (result.isSuccess) {
                        $("tr[data-itemid=" + itemId + "]").remove();
                    } 
                    alert(result.Msg);
                });
                xhr.error(function (result) {
                    alert(result.statusText);
                });
            }
        });

        // when we click on an option then send a search request to the server
        $('body').on('click', '.searchOnClick', function (event) {
            var selectedElement = event.currentTarget;
            var $selectedElement = $(event.currentTarget);

            // get the search field name
            var searchField = selectedElement.getAttribute('data-searchField');

            // get the id of the clicked/selected item
            var searchValue = selectedElement.getAttribute('data-id');

            // get the name/visible value of the selected criteria
            var selectedItemName = $selectedElement.html();

            // get the label/noncollapsible part
            var $labelElement = $('[data-labelFor=' + searchField + ']');

            //var $hiddenFieldElement = $('#SearchFilter_' + searchField);

            // if an OS is selected then load the list of manufacturers
            if (searchField == 'OsId') {
                // clean the previously selected value for the manufacturer
                $('#SearchFilter_ManufacturerId').val('');

                $.ajax({
                    url: 'GetManufacturers',
                    cache: false,
                    type: 'POST',
                    data: { idOs: searchValue },
                    success: function (data) {
                        var $manufacturersElement = $('#manufacturers');

                        // remove the previous values
                        $manufacturersElement.empty();

                        // anable the filter for selecting a manufacturer
                        $('#manufacturers-header').removeClass('disabled-filter');

                        // fill up the list of manufacturers
                        for (var i = 0; i < data.length; i++) {
                            var tempItem = data[i];
                            $manufacturersElement.append('<div data-searchfield="ManufacturerId" data-id="' + tempItem.Value + '" class="accordion-collapsible-item searchOnClick">' + tempItem.Text + '</div>');
                        }
                    }
                });
            };

            // if a manufacturer is selected then load the list of models
            if (searchField == 'ManufacturerId') {
                // clean the previously selected value for the model
                $('#SearchFilter_ModelId').val('');

                $.ajax({
                    url: 'GetModels',
                    cache: false,
                    type: 'POST',
                    data: { idManufacturer: searchValue },
                    success: function (data) {
                        var $modelsElement = $('#models');

                        // remove the previous values
                        $modelsElement.empty();

                        // anable the filter for selecting a model
                        $('#models-header').removeClass('disabled-filter');

                        // fill up the list of models
                        for (var i = 0; i < data.length; i++) {
                            var tempItem = data[i];
                            $modelsElement.append('<div data-searchfield="ModelId" data-id="' + tempItem.Value + '" class="accordion-collapsible-item searchOnClick">' + tempItem.Text + '</div>');
                        }
                    }
                });
            };

            // change the value of the hidden input so it will be send to the server on submit
            //$hiddenFieldElement.val(searchValue);

            // unselect the previuosly selected element
            $selectedElement.siblings().removeClass('selected-left-side-filter');

            // add additional class to the selected element so we can style it
            $selectedElement.addClass('selected-left-side-filter');

            // each time when a new value is selected replace the value in the 'selected' part of the label
            $labelElement.find('.selected-left-side-filter').html(selectedItemName);


            SearchPresenter.searchFilter[searchField] = searchValue;

            // submit the form
            GridAjax.submit(SearchPresenter.searchFilter);
        });

        var _selectTopFilter = function (event) {

            var $currentButton = $(event.currentTarget);

            // unselect the previous selected filter
            $currentButton.siblings().removeClass('selected-top-filter');

            // mark as selected the current filter
            $currentButton.addClass('selected-top-filter');

            // submit the form
            GridAjax.submit(SearchPresenter.searchFilter);
        }

        $('body').on('click', '#show-all', function (event) {
       
            SearchPresenter.searchFilter['AvailableID'] = null;
            // mark as selected the current filter and submit the form
            _selectTopFilter(event);
        });

        $('body').on('click', '#show-available', function (event) {
         
            SearchPresenter.searchFilter['AvailableID'] = 1;
           
            // mark as selected the current filter and submit the form
            _selectTopFilter(event);
        });

        $('body').on('click', '#show-not-available', function (event) {
         
            SearchPresenter.searchFilter['AvailableID'] = 0;
            
            // mark as selected the current filter and submit the form
            _selectTopFilter(event);
        });

        $('body').on('change', '#sort-by', function (event) {

        });

        $('body').on('click', '#sort-direction', function (event) {

        });

        $('body').on('keyup', '#search-text', function (event) {
            /*var searchText = event.currentTarget.value;
            // when the user types more that 2 characters execute a search request
            if (searchText.length > 2) {
                // set the search text in the hidden input so it will be sent to the server
                $('#SearchFilter_Name').attr('value', searchText);

                // submit so a request will be sent
                GridAjax.submit(SearchPresenter.searchFilter);
            }*/
        });

        $('body').on('click', '#clear-filters', function () {
            // todo: Is this the best way? The other option is to iterate all hidden input fields and remove the selected values
            // after that to iterate all visual filter elements and again remove the selected values 
            // and finally to submit the form without any filters so the initial set of results will be shown

            // reload the page so all previously selected filters will be lost
            location.reload();
        });

        // create the accordion
        $("#accordion").accordion({
            heightStyle: "content",
            collapsible: true,
            active: false
        });

        //Grid.init($('#deviceGrid'), $('#formSearch'), '/Device/SearchAjax');
        GridAjax.init('divicesGidAjax', $('#formSearch'), '/Device/SearchAjaxJson');
    }
}