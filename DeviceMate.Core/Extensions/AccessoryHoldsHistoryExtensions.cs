using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class AccessoryHoldsHistoryExtensions
    {
        public static AccessoryHistoryProxy ConvertToAccessoryHisoryProxy(this AccessoryHoldsHistory accessoryHoldHistory, User historyHolder = null)
        {
            AccessoryHistoryProxy accessoryHisory = new AccessoryHistoryProxy();

            accessoryHisory.Id = accessoryHoldHistory.Id;
            accessoryHisory.Accessory = accessoryHoldHistory.Accessory.ConvertToAccessoryProxy();
            accessoryHisory.Accessory.DateTaken = accessoryHoldHistory.HoldDate;
            accessoryHisory.Hold = new HoldProxy()
            {
                Email = accessoryHoldHistory.Email,
                IsBusy = accessoryHoldHistory.IsBusy,
                FullName = historyHolder != null ? historyHolder.Name : string.Empty,
                ImagePath = historyHolder != null ? historyHolder.PictureUrl : string.Empty,

                Team = new TeamProxy()
                {
                    Id = accessoryHoldHistory.TeamId,
                    Name = accessoryHoldHistory.Team.Name
                },
                Location = new LocationProxy()
                {
                    Id = accessoryHoldHistory.TownId,
                    Name = accessoryHoldHistory.Town.Name
                }
            };

            return accessoryHisory;
        }

        public static IList<AccessoryHistoryProxy> ConvertToAccessoryHisoryProxies(this IEnumerable<AccessoryHoldsHistory> accessoryHoldHistoryCollection,
                                                                                IEnumerable<User> historyHolders = null)
        {
            IList<AccessoryHistoryProxy> historyList = new List<AccessoryHistoryProxy>();
            if (accessoryHoldHistoryCollection != null)
            {

                foreach (AccessoryHoldsHistory historyItem in accessoryHoldHistoryCollection)
                {
                    User historyHolder = null;

                    if (historyHolders != null)
                    {
                        historyHolder = historyHolders.Where(hh => hh.Email == historyItem.Email).FirstOrDefault();
                    }

                    AccessoryHistoryProxy historyProxy = historyItem.ConvertToAccessoryHisoryProxy(historyHolder);

                    if (historyProxy != null)
                    {
                        historyList.Add(historyProxy);
                    }
                }
            }
            return historyList;
        }
    }
}
