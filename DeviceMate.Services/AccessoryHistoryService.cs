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
    public class AccessoryHistoryService : IAccessoryHistoryService
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly AccessoryHoldsHistoryRepo _historyRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public AccessoryHistoryService(IUserService userService)
        {
            _userService = userService;
            _context = new DeviceContext();
            _historyRepo = new AccessoryHoldsHistoryRepo(_context);
        }
        #endregion

        #region IDeviceHistoryService methods
        public AccessoryHistoryProxyList GetByAccessoryId(int accessoryId, int itemsPerPage, int maxItems)
        {
            IEnumerable<AccessoryHoldsHistory> historyEntries = _historyRepo.GetNoTracking(h => h.AccessoryId == accessoryId)
                                                                   .OrderByDescending(h => h.HoldDate)
                                                                    .Include("Town")
                                                                    .Include("Accessory.AccessoryType")
                                                                    .Include("Accessory.OSs")
                                                                    .Include("Accessory.Hold.Team")
                                                                    .Include("Accessory.Hold.Town")
                                                                    .AsEnumerable();

            AccessoryHistoryProxyList historyList = new AccessoryHistoryProxyList();

            historyEntries = PagingHelper<AccessoryHoldsHistory>.GetCurrentPage(historyEntries, historyList, itemsPerPage, maxItems);
            IList<User> historyHolders = _userService.GetByEmails(historyEntries.Select(he => he.Email).Distinct().ToList());
            historyList.History = historyEntries.ConvertToAccessoryHisoryProxies(historyHolders);

            return historyList;
        }
        #endregion
    }
}
