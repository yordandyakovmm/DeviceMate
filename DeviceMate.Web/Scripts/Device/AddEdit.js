$(document).ready(function () {
    ListLoader.init("#Device_OsId", "#Device_ManufacturerId", "#Device_ModelId");
    DevicePresenter.init();
});

var DevicePresenter = {
    init: function () {
        $("#formDevice").submit(function (ev) {
            if (!$(this).valid()) {
                ev.preventDefault()
            }
        });

        $(".show-mask").click(function () {
            $('body').append('<div id="mask"></div>');
            $('#mask').fadeIn(200);
        });

        $("#openModelPopup").click(function () {
            var manufacturerSelectedOption = $("#Device_ManufacturerId").find(":selected");
            var manufacturerId = manufacturerSelectedOption.val();

            if (manufacturerId == undefined) {
                DevicePresenter.showError("#errorPopup", "You should select a manufacturer!");
            }
            else {
                var newModelManufacturerLabel = $("#newModelManufacturer");
                newModelManufacturerLabel.attr("data-manufacturerId", manufacturerId);
                newModelManufacturerLabel.html("Manufacturer: " + manufacturerSelectedOption.text().trim());
                $("#addModel").show();
            }            
        });

        $("#openColorPopup").click(function () {
            $("#addColor").show();
        });

        $("#openManufacturerPopup").click(function () {

            var OSSelectedOption = $("#Device_OsId").find(":selected");
            var OSId = OSSelectedOption.val();

            if (OSId == undefined) {
                DevicePresenter.showError("#errorPopup", "You should select an OS!");
            }
            else {
                var newManufacturerOSLabael = $("#newManufacturerOS");
                newManufacturerOSLabael.attr("data-osid", OSId);
                newManufacturerOSLabael.html("OS: " + OSSelectedOption.text().trim());
                $("#addManufacturer").show();
            }
        });

        $("#openOsPopup").click(function () {
            $("#addOs").show();
        });

        $(".close").click(function () {
            $(".popup").hide();
            $('#mask').remove();
        });

        $("#addColor-button").click(function () {
            var name = $("#colorName").val();

            if (/\S/.test(name)) {
                var url = $(this).data('url');
                DevicePresenter.addColor(name, url, "Device_ColorId");
            } else {
                alert("Add a color name, please.");
            }            
        });

        $("#addOs-button").click(function () {
            
            var name = $("#osName").val();
            if (/\S/.test(name)) {
                var url = $(this).data('url');
                DevicePresenter.addOs(name, url, "Device_OsId");
            }
            else {
                alert("Add an OS name, please.");
            }            
        });

        $("#addModel-button").click(function () {
            var $this = $(this);
            var modelName = $("#modelName").val();
            var manufacturerId = $this.siblings("#newModelManufacturer").attr("data-manufacturerid");

            if (!/\S/.test(modelName)) {
                alert("Add a model name, please.");
            } else if (!/\S/.test(manufacturerId)) {
                alert("Model information is missing.");
            } else {
                var url = $(this).data('url');               

                var reloadParams = {
                    idManufacturer: manufacturerId
                };
                DevicePresenter.addModel(modelName, manufacturerId, url, "Device_ModelId", reloadParams);
            }
        });

        $("#addManufacturer-button").click(function () {
            console.log("dsadsadadasd")
            var $this = $(this);
            var manufacturerName = $("#manufacturerName").val();
            var osId = $this.siblings("#newManufacturerOS").attr("data-osid");

            if (!/\S/.test(manufacturerName)) {
                alert("Add a manufacturer name, please.");
            } else if (!/\S/.test(osId)) {
                alert("Manufacturer information is missing.");
            } else {
                var url = $this.data('url');

                var reloadParams = {
                    idOs: osId
                };

                DevicePresenter.addManufacturer(manufacturerName, osId, url, "Device_ManufacturerId", reloadParams);
            }            
        });

        $("#addScreenSize-button").click(function () {
            var size = $("#size").val();

            if (/^\d+(\.\d{1,2})?$/.test(size)) {
                var url = $(this).data('url');
                DevicePresenter.addScreenSize(size, url, "Device_ScreenSizeId");
            } else {
                alert("Add a screen size, please.");
            }
        });

        $("#addWidth-button").click(function () {
            var width = $("#width").val();

            if (/^\d+$/.test(width)) {
                var url = $(this).data('url');
                DevicePresenter.addWidth(width, url, "Device_ResolutionWidthId");
            } else {
                alert("Add a resolution width, please.");
            }
        });

        $("#addHeight-button").click(function () {
            var height = $("#height").val();

            if (/^\d+$/.test(height)) {
                var url = $(this).data('url');
                DevicePresenter.addHeight(height, url, "Device_ResolutionHeightId");
            } else {
                alert("Add a resolution height, please.");
            }
        });

        $("#deleteColor").click(function (e) {
            var id = $("#Device_ColorId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select color!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                DevicePresenter.deleteById(id, url, "Color", "Device_ColorId");
            }
        });

        $("#deleteModel").click(function (e) {
            var id = $("#Device_ModelId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select Model!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                var reloadParams = {
                    idManufacturer: $("#Device_ManufacturerId").find(":selected").val()
                };
                DevicePresenter.deleteById(id, url, "Model", "Device_ModelId", reloadParams);
            }
        });

        $("#deleteManufacturer").click(function (e) {
            var id = $("#Device_ManufacturerId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select Manufacturer!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                var reloadParams = {
                    idOs: $("#Device_OsId").find(":selected").val()
                };
                DevicePresenter.deleteById(id, url, "Manufacturer", "Device_ManufacturerId", reloadParams);
            }
        });

        $("#deleteOs").click(function (e) {
            var id = $("#Device_OsId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select Os!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                DevicePresenter.deleteById(id, url, "Os", "Device_OsId");
            }
        });

        $("#deleteScreenSize").click(function (e) {
            var id = $("#Device_ScreenSizeId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select Screen Size!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                DevicePresenter.deleteById(id, url, "ScreenSize", "Device_ScreenSizeId");
            }
        });

        $("#deleteWidth").click(function (e) {
            var id = $("#Device_ResolutionWidthId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select width!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                DevicePresenter.deleteById(id, url, "Width", "Device_ResolutionWidthId");
            }
        });

        $("#deleteHeight").click(function (e) {
            var id = $("#Device_ResolutionHeightId").find(":selected").val();

            if (id == undefined) {
                DevicePresenter.showError("#errorPopup", "Please select height!");
            } else if (!confirm("Are you sure you want to delete it?")) {
                $("#mask").remove();
                e.preventDefault();
            } else {
                var url = $(this).data('url');
                DevicePresenter.deleteById(id, url, "Height", "Device_ResolutionHeightId");
            }
        });

        $("#openScreenSizePopup").click(function () {
            $("#addScreenSize").show();
        });

        $("#openWidthPopup").click(function () {
            $("#addWidth").show();
        });

        $("#openHeightPopup").click(function () {
            $("#addHeight").show();
        });
    },

    addColor: function (name, url, selectToReload) {
        var xhr, params;
        params = {
            name: name
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#colorName").val("");
                SelectHelper.reloadSelect(selectToReload);
            } else {
                alert("Incorrect input, color saving failed.");
            }
        });
    },

    addOs: function (name, url, selectToReload) {
        var xhr, params;
        params = {
            name: name
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#osName").val("");
                $("#Device_ManufacturerId").html("");
                $("#Device_ModelId").html("");

                SelectHelper.reloadSelect(selectToReload);
            } else {
                alert("Incorrect input, OS saving failed.");
            }
        });
    },

    addModel: function (modelName, manufacturerId, url, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            manufacturerId: manufacturerId,
            name: modelName,
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });

        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();
                $("#modelName").val("");
                SelectHelper.reloadSelect(selectToReload, reloadParams);
            }
            else {
                alert("Incorrect input, device model saving failed.");
            }
        });


    },

    addManufacturer: function (manufacturerName, osId, url, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            osId: osId,
            name: manufacturerName,
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#manufacturerName").val("");
                $("#Device_ModelId").html("");

                SelectHelper.reloadSelect(selectToReload, reloadParams);
            }
            else {
                alert("Incorrect input, device manufacturer saving failed.");
            }
        });
    },

    addScreenSize: function (size, url, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            size: size
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#size").val("");

                SelectHelper.reloadSelect(selectToReload, reloadParams);
            }
            else {
                alert("Incorrect input, device screen size saving failed.");
            }
        });
    },

    addWidth: function (width, url, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            width: width,
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#width").val("");

                SelectHelper.reloadSelect(selectToReload, reloadParams);
            }
            else {
                alert("Incorrect input, device resolution width saving failed.");
            }
        });
    },

    addHeight: function (height, url, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            height: height,
        }
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.Success) {
                $(".close").click();

                $("#height").val("");

                SelectHelper.reloadSelect(selectToReload, reloadParams);
            }
            else {
                alert("Incorrect input, device resolution height saving failed.");
            }
        });
    },

    deleteById: function (id, url, messageName, selectToReload, reloadParams) {
        var xhr, params;
        params = {
            id: id
        };
        xhr = $.ajax({ url: url, data: params, type: "POST" });
        xhr.success(function (result) {
            if (result.deleted == true) {
                $("#mask").remove();
                if (reloadParams) {
                    SelectHelper.reloadSelect(selectToReload, reloadParams);
                } else {
                    SelectHelper.reloadSelect(selectToReload);
                }                
            } else {
                $("#errorPopup").show();
                $("#errorPopup p").html("Can't delete " + messageName + " in use!");
            }
        });
        xhr.error(function () {
            $("#errorPopup").show();
            $("#errorPopup p").html("Error!");
        });
    },

    showError: function (elementIdentifier, text) {
        $(elementIdentifier).show();
        $(elementIdentifier + " p").html(text);
    }
}