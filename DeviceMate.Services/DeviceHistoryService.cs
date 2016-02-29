using DeviceMate.Core.Extensions;
using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using DeviceMate.Services.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class DeviceHistoryService : IDeviceHistoryService
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly DeviceHoldsHistoryRepo _historyRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public DeviceHistoryService(IUserService userService)
        {
            _userService = userService;
            _context = new DeviceContext();
            _historyRepo = new DeviceHoldsHistoryRepo(_context);
        }
        #endregion

        #region IDeviceHistoryService methods
        public DeviceHistoryProxyList GetByDeviceId(int deviceId, int itemsPerPage, int maxItems)
        {
            IEnumerable<DeviceHoldsHistory> historyEntries = _historyRepo.GetNoTracking(h => h.DeviceId == deviceId)
                                                                   .OrderByDescending(h => h.HoldDate)
                                                                    .Include("Town")
                                                                    .Include("Device.DeviceType")
                                                                    .Include("Device.Model.Manufacturer.OSs")
                                                                    .Include("Device.Hold.Team")
                                                                    .Include("Device.Hold.Town")
                                                                    .Include("Device.ScreenSize")
                                                                    .Include("Device.Resolution.ResolutionHeightOption")
                                                                    .Include("Device.Resolution.ResolutionWidthOption")
                                                                    .AsEnumerable();

            DeviceHistoryProxyList historyList = new DeviceHistoryProxyList();

            historyEntries = PagingHelper<DeviceHoldsHistory>.GetCurrentPage(historyEntries, historyList, itemsPerPage, maxItems);
            IList<User> historyHolders = _userService.GetByEmails(historyEntries.Select(he => he.Email).Distinct().ToList());
            historyList.History = historyEntries.ConvertToDeviceHisoryProxies(historyHolders);

            return historyList;
        }
        #endregion
    }
}
