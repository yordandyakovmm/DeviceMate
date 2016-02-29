
namespace DeviceMate.Core
{
    public static class Common
    {
        /// <summary>
        /// The application business libraries prefix
        /// </summary>
        public const string ApplicationBusinessLibrariesPrefix = "DeviceMate.Service";

        /// <summary>
        /// The application relative path for external token.
        /// </summary>
        public const string ApplicationRelativePathToken = "/Token";

        /// <summary>
        /// The application relative path for authorize.
        /// </summary>
        public const string ApplicationRelativePathAuthorize = "/Account/Authorize";

        /// <summary>
        /// The application authentication type bearer
        /// </summary>
        public const string ApplicationAuthenticationTypeBearer = "Bearer";

        /// <summary>
        /// The application authentication cookie name
        /// </summary>
        public const string ApplicationAuthenticationCookieName = "DM.Auth";

        /// <summary>
        /// The default page size for paged items
        /// </summary>
        public const int DefaultPageSize = 20;

        /// <summary>
        /// The success message that will be sent with every successful response from the WebApi
        /// </summary>
        public const string SuccessMessage = "OK";

        /// <summary>
        /// The ID of the team that all holders will be transfered to when a team is removed.
        /// </summary>
        public const int DefaultTeamId = 1;

        /// <summary>
        /// Average cache time in minutes.
        /// </summary>
        public const int AvarageCacheTimeInMunites = 1440;

        /// <summary>
        /// The name of the cache entry with the last time when the employees data were updated.
        /// </summary>
        public const string EmployeeUpdateTimeCache = "EmployeeUpdateTime";

        /// <summary>
        /// The time in hours between refreshes of the employees data.
        /// </summary>
        public const int EmployeeUpdatePeriodInHours = 12;

        /// <summary>
        /// The prefix of the image file resources in the database.
        /// </summary>
        public const string ResourceFilePrefix = "file:";

        /// <summary>
        /// The permalink format for Google drive images
        /// </summary>
        public const string UserImagePattern = "/Content/images/ProfileImages/{0}";
        //public const string GoogleViewImagePattern = "http://drive.google.com/uc?export=view&id={0}";

        /// <summary>
        /// The Admin user role name.
        /// </summary>
        public const string AdminUserRole = "Admin";

        /// <summary>
        /// The days after which the login cookie will expire if the "Remember me" option was checked on login.
        /// </summary>
        public const int LoginCookieExpirationInDays = 365;
    }
}
