$(document).ready(function () {
    ListLoader.init("#SearchFilter_OsId", "#SearchFilter_ManufacturerId", "#SearchFilter_ModelId", "#SearchFilter_TownID");
    SearchPresenter.init();
});

var SearchPresenter = {

    clearFilter: function () {
        $("div.well input[type=text]").val("");
        $("div.well select").val([]);
    },

    init: function () {
        $("#btnClear").click(function () {
            SearchPresenter.clearFilter();
        });

        Grid.init($('#deviceHistoryGrid'), $('#formSearch'), '/Device/HistoryAjax');
    }
}