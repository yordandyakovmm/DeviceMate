using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.ProviderAuthenticationPolicy;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace DeviceMate.Web.Models
{
    public class OpenIdUser
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string FullName { get; set; }
        public bool IsSignedByProvider { get; set; }
        public string ClaimedIdentifier { get; set; }
        
        public OpenIdUser(string data)
        {
            populateFromDelimitedString(data);
        }

        public OpenIdUser(ClaimsResponse claim, string identifier)
        {
            addClaimInfo(claim, identifier);
        }

        private void addClaimInfo(ClaimsResponse claim, string identifier)
        {
            Email = claim.Email;
            FullName = claim.FullName;
            Nickname = claim.Nickname ?? claim.Email;
            IsSignedByProvider = claim.IsSignedByProvider;
            ClaimedIdentifier = identifier;
        }

        private void populateFromDelimitedString(string data)
        {
            if (data.Contains(";"))
            {
                var stringParts = data.Split(';');
                if (stringParts.Length > 0) Email = stringParts[0];
                if (stringParts.Length > 1) FullName = stringParts[1];
                if (stringParts.Length > 2) Nickname = stringParts[2];
                if (stringParts.Length > 3) ClaimedIdentifier = stringParts[3];
            }
        }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3}", Email, FullName, Nickname, ClaimedIdentifier);
        }
    }

    internal interface IOpenIdMembershipService
    {
        IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier);
        OpenIdUser GetUser();
    }

    public class OpenIdMembershipService : IOpenIdMembershipService
    {
        private readonly OpenIdRelyingParty openId;

        public OpenIdMembershipService()
        {
            openId = new OpenIdRelyingParty();
        }

        public IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier)
        {
            var openIdRequest = openId.CreateRequest(Identifier.Parse(openIdIdentifier));
            openIdRequest.Mode = AuthenticationRequestMode.Setup;
            openIdRequest.AddExtension(new ClaimsRequest
            {
                Email = DemandLevel.Require,
                FullName = DemandLevel.Require

            });
            openIdRequest.AddExtension(new PolicyRequest { MaximumAuthenticationAge = TimeSpan.Zero });


            return openIdRequest;
        }

        public OpenIdUser GetUser()
        {
            OpenIdUser user = null;
            var openIdResponse = openId.GetResponse();
            if (openIdResponse.IsSuccessful())
            {
                user = ResponseIntoUser(openIdResponse);
            }
            return user;
        }

        private OpenIdUser ResponseIntoUser(IAuthenticationResponse response)
        {
            OpenIdUser user = null;
            var claimResponseUntrusted = response.GetUntrustedExtension<ClaimsResponse>();
            var claimResponse = response.GetExtension<ClaimsResponse>();
            //var fetchResponse = response.GetExtension<FetchResponse>();

            if (claimResponse != null)
            {
                user = new OpenIdUser(claimResponse, response.ClaimedIdentifier);
            }
            else if (claimResponseUntrusted != null)
            {
                user = new OpenIdUser(claimResponseUntrusted, response.ClaimedIdentifier);
            }

            if (string.IsNullOrEmpty(user.Nickname))
            {
                user.Nickname = user.Email;
            }

            return user;
        }

        public HttpCookie CreateFormsAuthenticationCookie(OpenIdUser user)
        {
            var ticket = new FormsAuthenticationTicket(1, user.Nickname, DateTime.Now, DateTime.Now.AddHours(8), false, user.ToString());
            var encrypted = FormsAuthentication.Encrypt(ticket).ToString();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

            return cookie;
        }
    }

    public static class Extensions
    {
        public static bool IsSuccessful(this IAuthenticationResponse response)
        {
            return response != null && response.Status == AuthenticationStatus.Authenticated;
        }
    }

    public class OpenIdIdentity : IIdentity
    {
        private readonly OpenIdUser _user;

        public OpenIdIdentity(OpenIdUser user)
        {
            _user = user;
        }

        public OpenIdUser OpenIdUser
        {
            get
            {
                return _user;
            }
        }

        public string AuthenticationType
        {
            get
            {
                return "OpenID Identity";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return _user.Nickname ?? string.Empty;
            }
        }
    }

    public class OpenIdAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized)
            {
                var authenticatedCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authenticatedCookie != null)
                {
                    var authenticatedCookieValue = authenticatedCookie.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(authenticatedCookieValue))
                    {
                        var decryptedTicket = FormsAuthentication.Decrypt(authenticatedCookieValue);
                        var user = new OpenIdUser(decryptedTicket.UserData);
                        var openIdIdentity = new OpenIdIdentity(user);
                        httpContext.User = new GenericPrincipal(openIdIdentity, null);
                    }
                }
            }
            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}