
ListLoader = {

    os: "",
    manufacturers: "",
    models: "",

    onOsChange: function (event) {
        $.ajax({
            type: 'POST',
            url: $(ListLoader.manufacturers).data("url"),
            targetctrl: event.data.manufacturers,
            data: { idOs: $(this).val() },
            success: function (data) {
                fillSelectList(this.targetctrl, data);
                $(ListLoader.models).find("option").remove();

            }
        });
    },

    onManufacturersChange: function (event) {
        $.ajax({
            type: 'POST',
            url: $(ListLoader.models).data("url"),
            targetctrl: event.data.models,
            data: { idManufacturer: $(this).val() },
            success: function (data) {
                fillSelectList(this.targetctrl, data);
            }
        });
    },

    init: function (os, manufacturers, models) {
        this.os = os;
        this.manufacturers = manufacturers;
        this.models = models;
        $(this.os).change({
            manufacturers: this.manufacturers,
        }, this.onOsChange);
        $(this.manufacturers).change({
            models: this.models,
        }, this.onManufacturersChange);
    }
};

function fillSelectList(selector, data) {
    var markup = "";
    for (var x = 0; x < data.length; x++) {
        markup += "<option value='" + data[x].Value + "'>" + data[x].Text + "</option>";
    }
    $(selector).html(markup).show();
};
