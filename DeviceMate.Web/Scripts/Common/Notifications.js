$(document).ready(function () {
    var $successNotification = $("#successNotification")
    if ($successNotification.is(":visible")) {
        $successNotification.delay(2000).fadeOut("slow");
    }
});
