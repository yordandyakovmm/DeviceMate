using DMC = DeviceMate.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;

namespace DeviceMate.Web
{
    /// <summary>
    /// Extend the Owin Startup Class For Authorization Setup.
    /// </summary>
    public static partial class Startup
    {
        #region Fields
        private static readonly GoogleOAuth2AuthenticationOptions googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions()
        {
            ClientId = DMC.Config.GoogleOAuthClientId,
            ClientSecret = DMC.Config.GoogleOAuthClientSecret
        };

        private static readonly OAuthAuthorizationServerOptions authorizationServerOptions = new OAuthAuthorizationServerOptions()
        {
            AuthenticationType = DefaultAuthenticationTypes.ExternalBearer,
            TokenEndpointPath = new PathString(DMC.Common.ApplicationRelativePathToken),
            AuthorizeEndpointPath = new PathString(DMC.Common.ApplicationRelativePathAuthorize),
            Provider = new ApplicationOAuthProvider(),
            AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
            AllowInsecureHttp = true,
        };
        #endregion

        #region Configure
        /// <summary>
        /// Setup the OAuth Method.
        /// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864.
        /// </summary>
        public static void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = DMC.Common.ApplicationAuthenticationCookieName,
                LoginPath = new PathString("/Account/LogOn"),
                ExpireTimeSpan = new TimeSpan(DMC.Common.LoginCookieExpirationInDays, 0, 0, 0)
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(authorizationServerOptions);

            // Enable the Google authentication
            googleOAuth2AuthenticationOptions.Scope.Add("profile");
            googleOAuth2AuthenticationOptions.Scope.Add("email");
            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);
        }
        #endregion

        #region Inner Types
        /// <summary>
        /// Application OAuth Provider For Validate Request.
        /// </summary>
        private class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
        {
            /// <summary>
            /// Called to validate that the context.ClientId is a registered "client_id", and that the context.RedirectUri a "redirect_uri"
            /// registered for that client. This only occurs when processing the Authorize endpoint. The application MUST implement this
            /// call, and it MUST validate both of those factors before calling context.Validated. If the context.Validated method is called
            /// with a given redirectUri parameter, then IsValidated will only become true if the incoming redirect URI matches the given redirect URI.
            /// If context.Validated is not called the request will not proceed further.
            /// </summary>
            /// <param name="context">The context of the event carries information in and results out.</param>
            /// <returns>
            /// Task to enable asynchronous execution.
            /// </returns>
            public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
                else if (context.ClientId == "web")
                {
                    var expectedUri = new Uri(context.Request.Uri, "/");
                    context.Validated(expectedUri.AbsoluteUri);
                }

                return Task.FromResult<object>(null);
            }
        }
        #endregion
    }
}