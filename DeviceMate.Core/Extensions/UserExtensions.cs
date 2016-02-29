using DeviceMate.Core.Helpers;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class UserExtensions
    {
        public static UserProxy ConvertToUserProxy(this User user, IEnumerable<Device> userDevices = null, IEnumerable<DeviceHoldsHistory> userHistory = null)
        {
            if (user != null)
            {
                UserProxy userProxy = new UserProxy()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Skype = user.Skype,
                    PictureUrl = user.PictureUrl,
                    Position = user.Position,
                    IsAdmin = user.IsAdmin,
                    IsDeleted = user.StatusId == (int)enUserStatus.Inactive,
                    Location = !user.TownId.HasValue ? null : new LocationProxy()
                    {
                        Id = user.Town.TownId,
                        Name = user.Town.Name
                    },
                    Team = !user.TeamId.HasValue ? null : new TeamProxy()
                    {
                        Id = user.Team.Id,
                        Name = user.Team.Name
                    },

                    Devices = userDevices.ConvertToDeviceProxies(),

                    DeviceHistory = userHistory.ConvertToDeviceHisoryProxies()

                };

                return userProxy;
            }
            else
            {
                return null;
            }
        }

        public static IList<UserProxy> ConvertToUserProxies(this IEnumerable<User> users)
        {
            IList<UserProxy> userProxies = new List<UserProxy>();

            foreach (User user in users)
            {
                UserProxy userProxy = user.ConvertToUserProxy();
                if (userProxy != null)
                {
                    userProxies.Add(userProxy);
                }
            }

            return userProxies;
        }
    }
}
