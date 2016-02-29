using DeviceMate.Models.Enums;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DeviceMate.Core.Services.Membership
{
    /// <summary>
    /// Membership Login/Logout interface service.
    /// </summary>
    public interface IMembershipService
    {
        /// <summary>
        /// Re-Login to the system for OAuth.
        /// </summary>
        void Login(IPrincipal model);

        /// <summary>
        /// Log out of the system.
        /// </summary>
        void Logout();

        /// <summary>
        /// Login with external provider.
        /// </summary>
        Task<enLoginStatus> ExternalLogin(bool isPersistent);
    }
}