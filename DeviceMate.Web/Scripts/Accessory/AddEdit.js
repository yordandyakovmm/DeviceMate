$(document).ready(function () {
    AccessoryPresenter.init();
});

var AccessoryPresenter = {
    init: function () {
        $(".show-mask").click(function () {
            $('body').append('<div id="mask"></div>');
            $('#mask').fadeIn(200);
        });

        $("#openAccessoryTypePopup").click(function () {
            $("#addAccessoryType").show();
        });       

        $("#openAccessoryDescriptionPopup").click(function () {
            $("#addAccessoryDescription").show();
        });

        $(".close").click(function () {
            $(".popup").hide();
            $('#mask').remove();
        });

        $("#addAccessoryType-button").click(function () {
            var name = $("#accessoryTypeName").val();

            if (/\S/.test(name)) {
                var url = $(this).data('url');
                AccessoryPresenter.addAccessoryType(name, url);
            } else {
                alert("Add an accessory type name, please.");
            }
        });   

        $("#addAccessoryDescription-button").click(function () {
            var descirption = $("#accessoryDescription").val();

            if (/\S/.test(descirption)) {
                var url = $(this).data('url');
                AccessoryPresenter.addAccessoryDescription(descirption, url);
            } else {
                alert("Add an accessory description, please.");
            }
        });

        $("#deleteAccessoryType").click(function (e) {
            var selectId = "Accessory_AccessoryTypeId";
            var id = $("#" + selectId).find(":selected").val();

            if (id == undefined) {
                AccessoryPresenter.showError("#errorPopup", "Select an accessory type, please.");
            } else  if (!confirm("Are you sure you want to delete it?")){
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                AccessoryPresenter.deleteOptionById(id, url, selectId);
            }
        });

        $("#deleteAccessoryDescription").click(function (e) {
            var selectId = "Accessory_AccessoryDescriptionId";
            var id = $("#" + selectId).find(":selected").val();

            if (id == undefined) {
                AccessoryPresenter.showError("#errorPopup", "Select an accessory description, please.");
            } else if (!confirm("Are you sure you want to delete it?")) {
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                AccessoryPresenter.deleteOptionById(id, url, selectId);
            }
        });
    },

    addAccessoryType: function (name, url) {
        var xhr, params;
        params = {
            name: name
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.isSuccess) {
                $(".close").click();
                SelectHelper.reloadSelect("Accessory_AccessoryTypeId");
            }
            else {
                alert(result.errorMsg);
            }
        });
    },

    addAccessoryDescription: function (description, url) {
        var xhr, params;
        params = {
            description: description
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.isSuccess) {
                $(".close").click();
                SelectHelper.reloadSelect("Accessory_AccessoryDescriptionId");
            }
            else {
                alert(result.errorMsg);
            }
        });
    },

    deleteOptionById: function (id, url, selectElementId) {
        var xhr, params;
        params = {
            id: id
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.isSuccess) {
                SelectHelper.reloadSelect(selectElementId);
            }
            else {
                alert(result.errorMsg);
            }
        });
    },  

    showError: function (elementIdentifier, text) {
        $(elementIdentifier).show();
        $(elementIdentifier + " p").html(text);
    }
}