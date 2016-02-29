using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class DeviceHoldsHistoryExtensions
    {
        public static DeviceHistoryProxy ConvertToDeviceHisoryProxy(this DeviceHoldsHistory deviceHoldHistory, User historyHolder = null)
        {
            DeviceHistoryProxy deviceHisory = new DeviceHistoryProxy();

            deviceHisory.Id = deviceHoldHistory.Id;
            deviceHisory.Device = deviceHoldHistory.Device.ConvertToDeviceProxy();
            deviceHisory.Device.DateTaken = deviceHoldHistory.HoldDate;
            deviceHisory.Hold = new HoldProxy()
            {
                Email = deviceHoldHistory.Email,
                IsBusy = deviceHoldHistory.IsBusy,
                FullName = historyHolder != null ? historyHolder.Name : string.Empty,
                ImagePath = historyHolder != null ? historyHolder.PictureUrl : string.Empty,
                
                Team = new TeamProxy()
                {
                    Id = deviceHoldHistory.TeamId,
                    Name = deviceHoldHistory.Team.Name
                },
                Location = new LocationProxy()
                {
                    Id = deviceHoldHistory.TownID,
                    Name = deviceHoldHistory.Town.Name
                }
            };

            return deviceHisory;
        }

        public static IList<DeviceHistoryProxy> ConvertToDeviceHisoryProxies(this IEnumerable<DeviceHoldsHistory> deviceHoldHistoryCollection,
                                                                                IEnumerable<User> historyHolders = null)
        {
            IList<DeviceHistoryProxy> historyList = new List<DeviceHistoryProxy>();
            if (deviceHoldHistoryCollection != null)
            {

                foreach (DeviceHoldsHistory historyItem in deviceHoldHistoryCollection)
                {
                    User historyHolder = null;

                    if (historyHolders != null)
                    {
                        historyHolder = historyHolders.Where(hh => hh.Email == historyItem.Email).FirstOrDefault();
                    }

                    DeviceHistoryProxy historyProxy = historyItem.ConvertToDeviceHisoryProxy(historyHolder);

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
