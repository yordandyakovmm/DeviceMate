var app = function () {
    var my = {
        accessTokenKeyInStorege: "accessToken",
        parseQueryString: function () {
            /// <summary>Parse the hash params</summary>
            if (window.location.hash.indexOf("#") !== 0) return {};

            var data = {},
                queryString = window.location.hash.substr(1),
                pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

            if (queryString === null) {
                return data;
            }

            pairs = queryString.split("&");

            for (var i = 0; i < pairs.length; i++) {
                pair = pairs[i];
                separatorIndex = pair.indexOf("=");

                if (separatorIndex === -1) {
                    escapedKey = pair;
                    escapedValue = null;
                } else {
                    escapedKey = pair.substr(0, separatorIndex);
                    escapedValue = pair.substr(separatorIndex + 1);
                }

                key = decodeURIComponent(escapedKey);
                value = decodeURIComponent(escapedValue);

                data[key] = value;
            }

            return data;
        },
        getNewAccessToken: function () {
            /// <summary>Request a new access token from the server or the returned hash</summary>
            var fragment = my.parseQueryString();
            if (fragment.access_token) {
                // returning with access token, restore old hash, or at least hide token
                window.location.hash = fragment.state || '';
                sessionStorage.setItem(my.accessTokenKeyInStorege, fragment.access_token);
                return fragment.access_token;
            } else {
                // no token - so bounce to Authorize endpoint in AccountController to sign in or register
                window.location = "/Account/Authorize?client_id=web&response_type=token&state=" + encodeURIComponent(window.location.hash);
            }
        },
        removeAccessTokenFromStorage: function () {
            /// <summary>Remove the access token from session storage</summary>
            sessionStorage.removeItem(my.accessTokenKeyInStorege);
        }
    };

    return {
        init: function () {
            this.content = document.querySelector("[role=main]");
            if (this.content) {
                this.accessToken = sessionStorage.getItem(my.accessTokenKeyInStorege) || my.getNewAccessToken();
            }
        }
    };
}();

app.init();