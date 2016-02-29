$(document).ready(function () {
    AccessorySearchPresenter.init();
});

var AccessorySearchPresenter = {
    clearFilter: function () {
        $("div.well input[type=text]").val("");
        $("div.well select").val([]);
    },

    init: function () {
        $("#btnClear").click(function () {
            AccessorySearchPresenter.clearFilter()
        });

        $(".btn.delete").click(function (event) {
            if (!confirm("Are you sure you want to delete the accessory?")) {
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
                    } else {
                        alert(result.Msg);
                    }
                });
                xhr.error(function (result) {
                    alert(result.statusText);
                });
            }
        });

        Grid.init($('#accessoryGrid'), $('#formDevice'), '/Accessory/SearchAjax');
        //GridAjax.init('divicesGidAjax', $('#formSearch'), '/Accessory/SearchAjax');
    },
}