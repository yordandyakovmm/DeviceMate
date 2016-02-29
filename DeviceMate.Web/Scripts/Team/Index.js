$(document).ready(function () {
    TeamPresenter.init();
});

var TeamPresenter = {
    init: function () {
        $(".btn.delete").click(function (event) {
            console.log("here")
            if (!confirm("Are you sure you want to delete the Team?")) {
                event.preventDefault();
            }
            else {
                var url = $(this).data("url");
                var itemId = $(this).data("item-id");
                var xhr, params;
                params = {
                    id: itemId
                }

                xhr = $.ajax({ url: url, data: params, type: "POST" });
                xhr.success(function (result) {
                    if (result.isSuccess) {
                        $("tr[data-item-id=" + itemId + "]").remove();
                    } else {
                        if (result.Msg == undefined) {
                            window.location.reload();
                        }
                        else {
                            alert(result.Msg);
                        }
                    }
                });
                xhr.error(function (result) {
                    alert(result.statusText);
                });
            }
        });
    },
}