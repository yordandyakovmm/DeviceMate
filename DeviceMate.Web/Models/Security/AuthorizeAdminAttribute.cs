using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWS = System.Web.Security;

namespace DeviceMate.Web.Models.Security
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;
            bool IsUserInRole = SWS.Roles.Provider.IsUserInRole(user.Identity.Name, "Admin");
            return IsUserInRole;
        }
    }
}