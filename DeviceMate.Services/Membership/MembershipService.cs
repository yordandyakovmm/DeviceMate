using DeviceMate.Core;
using DeviceMate.Core.Services;
using DeviceMate.Core.Services.Membership;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace DeviceMate.Services.Membership
{
    /// <summary>
    /// <see cref="IMembershipService" /> Service Implementation.
    /// </summary>
    public class MembershipService : IMembershipService
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipService"/> class.
        /// </summary>
        public MembershipService()
            : this(new HttpContextWrapper(HttpContext.Current).GetOwinContext().Authentication,
                    new UserService()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipService"/> class.
        /// </summary>
        /// <param name="authenticationManager">The authentication manager.</param>
        public MembershipService(
            IAuthenticationManager authenticationManager,
            IUserService userService)
        {
            AuthenticationManager = authenticationManager;
            _userService = userService;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets Manager that handle the authentication process.
        /// </summary>
        /// <value>
        /// The authentication manager.
        /// </value>
        protected IAuthenticationManager AuthenticationManager { get; private set; }
        #endregion

        #region IMembershipService Members
        /// See Interface For More
        public async Task<enLoginStatus> ExternalLogin(bool isPersistent = false)
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            
            if (loginInfo != null &&
                loginInfo.ExternalIdentity != null &&
                !string.IsNullOrEmpty(loginInfo.Email) &&
                loginInfo.Email.EndsWith(Config.AllowedEmailDomain))
            {
                User user = await GetUser(loginInfo.Email);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                if (user.StatusId == (int)enUserStatus.Active)
                {
                    IEnumerable<Claim> claims = GetUserClaims(user);

                    AuthenticationManager.SignIn(
                        new AuthenticationProperties() { IsPersistent = isPersistent },
                        new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));
                    //new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie));

                    return enLoginStatus.Success;
                }
                else
                {
                    return enLoginStatus.UserInactive;
                }
            }
            else if (loginInfo == null || loginInfo.ExternalIdentity == null || string.IsNullOrEmpty(loginInfo.Email))
            {
                return enLoginStatus.Error;
            }
            else
            {
                return enLoginStatus.UserInvalid;
            }
        }

        /// See Interface For More
        public void Login(IPrincipal model)
        {
            var claims = new ClaimsPrincipal(model).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, Common.ApplicationAuthenticationTypeBearer);
            AuthenticationManager.SignIn(identity);
        }

        /// See Interface For More
        public void Logout()
        {
            AuthenticationManager.SignOut();
        }
        #endregion

        #region Helper Methods

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();

            
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, string.IsNullOrEmpty(user.Name) ? string.Empty : user.Name ));

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, enEmployeeRole.Admin.ToString()));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, enEmployeeRole.User.ToString()));
            }

            return claims;
        }

        private async Task<User> GetUser(string email)
        {
            await _userService.UpdateFromEmplyeeData(email);

            User user = _userService.GetByEmail(email);

            if (user == null)
            {
                user = await _userService.Add(email);
            }

            return user;
        }

        #endregion
    }
}
