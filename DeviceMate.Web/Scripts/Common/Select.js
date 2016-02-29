SelectHelper = {
    reloadSelect: function (selectElementId, params) {
        var url = $("#" + selectElementId).data("url");
        var xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            SelectHelper.populateSelect(selectElementId, result);
        });
    },

    populateSelect: function (selectIdentifier, options) {
        var select = $("#" + selectIdentifier);
        select.html("");
        $.each(options, function () {
            select.append($("<option></option>")
                .attr("value", this.Value)
                .text(this.Text));
        });
    }
}