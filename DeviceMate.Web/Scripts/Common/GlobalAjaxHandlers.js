$(document).ajaxError(function (xhr, ajaxOptions, thrownError) {
    switch (ajaxOptions.status) {
        case 403: // Unauthoruze AJAX Request
            window.location = '/';
    }
});