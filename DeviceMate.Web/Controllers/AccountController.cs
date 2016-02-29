using DeviceMate.Core.Services.Membership;
using DeviceMate.Models.Enums;
using DeviceMate.Web.Controllers.Abstract;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DeviceMate.Web.Controllers
{
    public class AccountController : BaseController
    {

        #region Services
        /// <summary>
        /// Gets or sets the membership service.
        /// </summary>
        /// <value>
        /// The membership service.
        /// </value>
        public IMembershipService MembershipService { get; set; }
        #endregion

        #region Fields
        //private readonly OpenIdMembershipService _openidemembership;
        #endregion

        #region Constructor

        public AccountController()
        {
            //_openidemembership = new OpenIdMembershipService();
        }

        #endregion

        #region Actions

        /// <summary>
        /// The Authorize Action is the end point which gets called when you access any
        /// protected Web API. If the user is not logged in then they will be redirected to the Login page.
        /// </summary>
        [HttpGet]
        public ActionResult Authorize()
        {
            MembershipService.Login(User);
            return new EmptyResult();
        }

        /// <summary>
        /// Perform a Logout Operation.
        /// </summary>
        [Authorize]
        public ActionResult Logout()
        {
            MembershipService.Logout();
            return RedirectToAction("LogOn", "Account");
        }

        /// <summary>
        /// External login authenticate.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public ActionResult ExternalLogin(string provider, bool persistent)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { persistent = persistent }));
        }


        /// <summary>
        /// Externals the login callback.
        /// </summary>
        public async Task<ActionResult> ExternalLoginCallback()
        {
            bool isPersistent = Request["persistent"] != null && Request["persistent"].ToLower() == bool.TrueString.ToLower();

            enLoginStatus loginStatus = await MembershipService.ExternalLogin(isPersistent);

            if (loginStatus == enLoginStatus.Success)
            {
                return RedirectToLocal(null);
            }
            else
            {
                return RedirectToAction("LogOn", "Account", new { status = loginStatus });
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult LogOn(string openid_identifier)
        //{
        //    var request = _openidemembership.ValidateAtOpenIdProvider(openid_identifier);

        //    if (request != null)
        //    {
        //        return request.RedirectingResponse.AsActionResult();
        //    }

        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult LogOn(enLoginStatus? status = null)
        {
            //OpenIdUser user = _openidemembership.GetUser();

            //if (user != null)
            //{
            //    if (user.Email.EndsWith("@mentormate.com"))
            //    {
            //        HttpCookie cookie = _openidemembership.CreateFormsAuthenticationCookie(user);
            //        if (Request["RememberMe"] != null &&
            //            Request["RememberMe"].ToLower() == bool.TrueString.ToLower())
            //        {
            //            cookie.Expires = DateTime.UtcNow.AddYears(1);
            //        }

            //        HttpContext.Response.Cookies.Add(cookie);

            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        ViewBag.Error = "You should log in with a mentormate account.";
            //    }
            //}

            Session["login"] = "login";

            if (status.HasValue && status != enLoginStatus.Success)
            {
                switch (status)
                {
                    case enLoginStatus.UserInvalid:
                        ViewBag.ErrorMessage = "Your account is invalid. Only MentorMate LLC employees are allowed to the application.";
                        break;
                    case enLoginStatus.UserInactive:
                        ViewBag.ErrorMessage = "Your account has been deactivated by a DeviceMate administrator.";
                        break;
                    case enLoginStatus.Error:
                        ViewBag.ErrorMessage = "System error occured. Please, try again after a few minutes.";
                        break;
                }
            }

            return View();
        }

        //public ActionResult LogOff()
        //{
        //    FormsAuthentication.SignOut();
        //    Session.Clear();  // This may not be needed -- but can't hurt
        //    Session.Abandon();

        //    // Clear authentication cookie
        //    HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        //    rFormsCookie.Expires = DateTime.Now.AddYears(-1);
        //    Response.Cookies.Add(rFormsCookie);

        //    // Clear session cookie 
        //    HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
        //    rSessionCookie.Expires = DateTime.Now.AddYears(-1);
        //    Response.Cookies.Add(rSessionCookie);
        //    // Invalidate the Cache on the Client Side
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetNoStore();
        //    return RedirectToAction("Index", "Home");
        //}

        #endregion

        #region Inner Methods
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Inner Types
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }    
}
