using System;
using System.Configuration;

namespace DeviceMate.Core
{
    public static class Config
    {
        public static string dbConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DeviceContext"].ConnectionString;
            }
        }

        #region Google APIs configuration
        public static string GoogleOAuthClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleOAuthClientId"];
            }
        }

        public static string GoogleOAuthClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleOAuthClientSecret"];
            }
        }

        public static string GoogleOAuthScope
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleOAuthScope"];
            }
        }

        public static string GoogleOAuthRedirectUri
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleOAuthRedirectUri"];
            }
        }

        public static string GoogleOAuthSheetsEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleOAuthSheetsEmail"];
            }
        }

        public static string GoogleOAuthCertPath
        {
            get
            {
                return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["GoogleOAuthCertPath"]);
            }
        }
        #endregion

        /// <summary>
        /// The directory where the profile images of the users are saved.
        /// </summary>
        public static string ImagesDirectoryPath
        {
            get
            {
                return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ImagesDirectoryPath"]);
            }
        }

        /// <summary>
        /// The e-mail domain allowed to log into the system.
        /// </summary>
        public static string AllowedEmailDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["AllowedEmailDomain"];
            }
        }

        /// <summary>
        /// The name of Spok team in the DB.
        /// </summary>
        public static string SpokTeamName
        {
            get
            {
                return ConfigurationManager.AppSettings["SpokTeamName"];
            }
        }

        /// <summary>
        /// The name of Spok team in the DB.
        /// </summary>
        public static int DefaultTeamId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DefaultTeamId"]);
            }
        }

        /// <summary>
        /// The name of the "no-team" team in the DB.
        /// </summary>
        public static string DefaultTeamName
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultTeamName"];
            }
        }
    }
}
