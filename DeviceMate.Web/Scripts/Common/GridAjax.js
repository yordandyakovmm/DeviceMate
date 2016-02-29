var GridAjax = {
    tableID: '',
    $form: '',
    url: '',

    init: function (tableID, $form, url) {
        this.tableID = tableID;
        this.$form = $form;
        this.url = url;
              
        GridAjax.$form.submit(function (e) {
            return GridAjax.submitForm(e);
        });

        GridAjax.submit();
        
    },

    submitForm: function (e, afterSuccess) {

        GridAjax.submit(afterSuccess);
        e.preventDefault();
        return false;
    },

    submit: function (searchFilter) {
        GridAjax.ajaxPost(searchFilter);
    },


    ajaxPost: function (formData, afterSuccess) {
        var self = this;
        $.ajax({
            url: GridAjax.url,
            cache: false,
            type: 'POST',
            data: formData,
            success: function (data) {
                GridAjax.showData(data);
				// mark as selected some of the default filters
                self.afterSubmit();
            }
        });
    },

    showData: function(devices) {
        var $parent = $('#' + GridAjax.tableID).parent();
        $parent.html('');
        $parent.prepend('<table id="divicesGidAjax"></table>');
        var data = devices;
        window.datatable = $('#' + GridAjax.tableID).DataTable({
            "data": data,
            "dom": '<"container-fluid" <"row-fluid"<"span3 availability-buttons"><"span6 pull-right"<"pull-right sort-direction-button"><"pull-right sort-by-button"><"pull-right" p><"pull-right" l>>><"row-fluid" f>>t',
            "columns": [
                { "title": "Number", data: 'Number' },
                { "title": "OsVersion", data: 'OsVersion' },
                { "title": "DeviceType_Name", data: 'DeviceType_Name' },
                { "title": "Name", data: 'Name' },
                { "title": "Resolution_Height", data: 'Resolution_Height' },
                { "title": "Resolution_Width", data: 'Resolution_Width' },
                { "title": "ScreenSize", data: 'ScreenSize' },
                { "title": "Color", data: 'Color' },
                { "title": "Hold_Team_Name", data: 'Hold_Team_Name' },
                { "title": "DeviceType_Name", data: 'DeviceType_Name' },
                { "title": "Hold_IsBusy", data: 'Hold_IsBusy' },
                { "title": "Hold_Town_Name", data: 'Hold_Town_Name' },
                { "title": "DeviceType_Name", data: 'DeviceType_Name' },
                { "title": "Model_Name", data: 'Model_Name' }

            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //nRow.cells[0].innerText = aData.Number + '- xxxxxx';
                return nRow;
            },
            "sPaginationType": "full_numbers",
            "bSort": true
        });
        $('.availability-buttons').html('<button id="show-all" type="button" class="no-border-button top-filter-button">Show All</button>'+
                                        '<button id="show-available" type="button" class="no-border-button top-filter-button">Available</button>'+
                                        '<button id="show-not-available" type="button" class="no-border-button top-filter-button">Not Available</button>');

        $('.sort-direction-button').html('<img id="sort-direction" src="/Content/images/icon-asc.png">');

        $('.sort-by-button').html('<span class="add-margin-right">Sort By</span>'+
                                            '<select id="sort-by">'+
                                                '<option value="">Choose</option>'+
                                                '<option value="Model.Name">Model</option>'+
                                                '<option value="Name">Name</option>'+
                                                '<option value="DeviceType.Name">Type</option>'+
                                                '<option value="Model.Manufacturer.OSs.Name">OS</option>'+
                                                '<option value="Hold.Team.Name">Team</option>'+
                                            '</select>');
    },

    afterSubmit: function () {
        // "restore" the query text in the input
        var searchText = $('#SearchFilter_Name').attr('value');
        if (searchText) {
            $('#search-text').attr('value', searchText);
        }

        var selectedValForAvailability = SearchPresenter.searchFilter['AvailableID'];

        if (selectedValForAvailability == '1') {
            $('#show-available').addClass('selected-top-filter');
        }
        else if (selectedValForAvailability == '0') {
            $('#show-not-available').addClass('selected-top-filter');
        }
        else {
            $('#show-all').addClass('selected-top-filter');
        }
    }

}
