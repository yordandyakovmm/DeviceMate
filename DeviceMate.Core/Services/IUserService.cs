using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceMate.Core.Services
{
    public interface IUserService
    {
        UserProxyList GetByFilter(UserFilter filter);
        UserProxy GetUserProxyById(int id, bool getHistory = false);
        UserProxy GetUserProxyByEmail(string email);

        User GetByEmail(string email);
        IList<User> GetByEmails(IList<string> emails);
        User GetById(int id);
        Task<User> Save(User user);
        Task<User> Add(string email);
        bool CheckIfExists(string email);
        bool SetAdminStatus(int userId, bool isAdmin, int? teamId = null);
        Task<bool> UpdateFromEmplyeeData(string email);
        Task<bool> UpdateFromEmplyeeData(IEnumerable<string> emails = null, bool ignoreTimeout = false, bool createMissingUsers = false);
        bool SetStatus(int userId,  enUserStatus status);
        bool Delete(int userId);
    }
}
