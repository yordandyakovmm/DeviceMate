using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace DeviceMate.Web.Models.Security
{
    public class DeviceMateRoleProvider : RoleProvider
    {
        #region Properties
        public RoleRepo RoleRepo { get; set; }

        public UserRepo UserRepo { get; set; }
        #endregion

        #region Public methods

        #region Constructor

        public DeviceMateRoleProvider()
        {
            DeviceContext context = new DeviceContext();
            RoleRepo = new RoleRepo(context);
            UserRepo = new UserRepo(context);
        }

        #endregion

        public override string ApplicationName
        {
            get
            {
                return DeviceMate.Objects.Helpers.Constants.Application.Name;
            }
            set
            {
            }
        }

        public override string[] GetAllRoles()
        {
            return RoleRepo.GetAll().Select(r => r.Name).ToList().ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            string role = DeviceMate.Objects.Helpers.Constants.Roles.User;
            User user = UserRepo.Get(u => u.Name == username && u.IsAdmin).FirstOrDefault();
            if (user != null)
            {
                role = DeviceMate.Objects.Helpers.Constants.Roles.Admin;
            }
            string[] ret = { role };
            return ret;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName == DeviceMate.Objects.Helpers.Constants.Roles.Admin)
            {
                return UserRepo.Get(u => u.IsAdmin).Select(u => u.Name).ToArray();
            }
            else
            {
                return new List<string>().ToArray();
            }
        }

        public override bool IsUserInRole(string userEmail, string roleName)
        {
            if (roleName == DeviceMate.Objects.Helpers.Constants.Roles.User)
            {
                return true;
            }
            else
            {
                return UserRepo.GetNoTracking().Any(u => u.Email == userEmail && u.IsAdmin);
            }
        }

        public override bool RoleExists(string roleName)
        {
            return RoleRepo.RoleExists(roleName);
        }

        #endregion

        #region Not implemented

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}