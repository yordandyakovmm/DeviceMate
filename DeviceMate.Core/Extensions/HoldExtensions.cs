using DeviceMate.Core.Helpers;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;

namespace DeviceMate.Core.Extensions
{
    public static class HoldExtensions
    {
        public static HoldProxy ConvertToHoldProxy(this Hold hold, User user)
        {
            if (hold != null)
            {
                HoldProxy holdProxy = new HoldProxy()
                {
                    Id = hold.Id,
                    Email = hold.Email,
                    IsBusy = hold.IsBusy,
                    FullName = user != null ? user.Name : string.Empty,
                    ImagePath = user != null ? user.PictureUrl : string.Empty,
                    Skype = user != null ? user.Skype : string.Empty,
                    Team = new TeamProxy()
                    {
                        Id = hold.TeamId,
                        Name = hold.Team.Name
                    },
                    Location = new LocationProxy()
                    {
                        Id = hold.TownID,
                        Name = hold.Town.Name
                    }
                };

                return holdProxy;
            }
            else
            {
                return null;
            }
        }
    }
}
