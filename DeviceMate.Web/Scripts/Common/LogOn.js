$(function() {
  //openid.init('openid_identifier');

  var chkRememberMe = $("#chkRememberMe");
  var $lnkLogin = $("#lnkLogIn");
  var logInLink = $lnkLogin.attr("href");
  var persistentRegEx = /persistent=(true|false)/i;

  if (logInLink.indexOf(persistentRegEx) > -1) {
    logInLink = logInLink.replace(persistentRegEx, "persistent=" + chkRememberMe.prop("checked"));
  } else {
    logInLink += "&persistent=" + chkRememberMe.prop("checked");
  }

  $lnkLogin.on('click', function(event) {
    // event.preventDefault();
    $lnkLogin
      .addClass('clicked')
      .find('i.icon')
      .addClass('animate-spin icon-spin5')
      .removeClass('icon-gplus');
  });

  $lnkLogin.attr("href", logInLink);
  //var url = window.location.href;
  //if (url.indexOf("RememberMe=true") !== -1) {
  //    rememberMeCb.attr("checked", "checked");
  //}

  $("#chkRememberMe").click(function() {
    var $this = $(this);
    var logInLink = $lnkLogin.attr("href");
    logInLink = logInLink.replace(/persistent=(true|false)/gi, "persistent=" + $this.prop("checked"));
    $lnkLogin.attr("href", logInLink);
  });

});

//function updateQueryString(key, value, url) {
//    if (!url) url = window.location.href;
//    var re = new RegExp("([?|&])" + key + "=.*?(&|#|$)(.*)", "gi");
//    if (re.test(url)) {
//        if (typeof value !== 'undefined' && value !== null)
//            return url.replace(re, '$1' + key + "=" + value + '$2$3');
//        else {
//            return url.replace(re, '$1$3').replace(/(&|\?)$/, '');
//        }
//    }
//    else {
//        if (typeof value !== 'undefined' && value !== null) {
//            var separator = url.indexOf('?') !== -1 ? '&' : '?',
//                hash = url.split('#');
//            url = hash[0] + separator + key + '=' + value;
//            if (hash[1]) url += '#' + hash[1];
//            return url;
//        }
//        else
//            return url;
//    }
//}
