$(document).ready(function () {
    AdministratorPresenter.init();
});

var AdministratorPresenter = {
    init: function () {
        $(".btn.delete").click(function (event) {
            if (!confirm("Are you sure you want to delete the User?")) {
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
        $('#UpdateAll').on('click', function (event) {
            var loadingIndicator;

            if (confirm('Warning! All user data will be overwritten. Are you sure you want to proceed?')) {
                loadingIndicator = LoadingHelper.showIcon(this);
                AdministratorPresenter.disableEditControls();

                xhr = $.ajax({
                    url: '/api/v1/users/updateall',
                    type: "GET"
                });
                xhr.success(function (result) {
                    console.log("Its UPDATED !")
                    window.location.reload();
                });
                xhr.error(function (result) {
                    alert(result.statusText);
                });
                xhr.always(function () {
                    LoadingHelper.hideIcon(loadingIndicator);
                    AdministratorPresenter.enableEditControls();
                });
            }
        });
        // update data for one user in the DB from the google file
        $('.update-user').on('click', function (event) {
            // get the user id
            var userId = event.currentTarget.dataset.itemId,
                loadingIndicator;

            // first open a confirmation dialog
            if (confirm('Warning! Data for this user will be overwritten. Are you sure you want to proceed?')) {
                loadingIndicator = LoadingHelper.showIcon(this);
                AdministratorPresenter.disableEditControls();

                xhr = $.ajax({
                    url: '/api/v1/users/update/' + userId,
                    type: "GET"
                });
                xhr.success(function (result) {
                    console.log("User " + userId +" is UPDATED !")
                    window.location.reload();
                });
                xhr.error(function (result) {
                    alert(result.statusText);
                });
                xhr.always(function () {
                    LoadingHelper.hideIcon(loadingIndicator);
                    AdministratorPresenter.enableEditControls();
                });
            }
        });
        $(document).on('click', 'js-disable-click', function(event) {
            event.preventDefault();
        });
    },
    disableEditControls: function() {
        var $items =  $('.js-disable-on-db-update');

        $items.each(function(i,elem) {
            $(elem).addClass('js-disable-click disabled');

            /** Only remove disabled attr from buttons */
            if (elem.tagname !== 'A') {
              elem.setAttribute('disabled', '');
            }
        });
    },
    enableEditControls: function() {
        var $items =  $('.js-disable-on-db-update');

        $items.each(function(i,elem) {
            $(elem).removeClass('js-disable-click disabled');

            /** Only remove disabled attr from buttons */
            if (elem.tagname !== 'A') {
                elem.removeAttribute('disabled');
            }
        });
    }
}
