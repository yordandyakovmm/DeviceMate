using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Enums;
using DeviceMate.Web.Areas.Api.Helpers;
using System;
using System.Web.Http;
using DMC = DeviceMate.Core;
using DME = DeviceMate.Models.Entities;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize]
    public class DevicesController : ApiController
    {
        #region Fields

        private readonly IDeviceService _deviceService;
        private readonly IUserService _userService;
        private readonly IDeviceHistoryService _historyService;

        #endregion

        #region Constructor
        public DevicesController(
            IDeviceService deviceService,
            IUserService userService,
            IDeviceHistoryService hisroyService
            )
            : base()
        {
            _deviceService = deviceService;
            _userService = userService;
            _historyService = hisroyService;
        }

        #endregion

        #region HTTP Actions
        [HttpGet]
        public DeviceProxyList List([FromUri] DeviceFilter filter)
        {
            filter.Keywords = QueryParamsHelper.GetKeywords(filter.Keyword);
            filter.Sort = QueryParamsHelper.GetSort(filter.SortColumns);

            if (!filter.Offset.HasValue)
            {
                filter.Offset = 0;
            }

            DeviceProxyList deviceList = _deviceService.GetByFilter(filter);
            deviceList.Message = DMC.Common.SuccessMessage;
            return deviceList;
        }

        [HttpGet]
        public DeviceProxy Show(int id)
        {
            DeviceProxy device = _deviceService.GetById(id);
            device.Message = DMC.Common.SuccessMessage;
            return device;
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Remove(int id)
        {
            _deviceService.Delete(id);
            
            return new ResponseMessage()
            {
                Message = DMC.Common.SuccessMessage
            };
        }

        [HttpPost]
        public ResponseMessage Submit(int id, HoldProxy hold)
        {
            if (hold == null)
            {
                throw new ArgumentNullException("hold");
            }

            DME.User user = _userService.GetByEmail(User.Identity.Name);

            if ((hold.Location == null || hold.Location.Town == enTown.Unknown))
            {
                hold.Location = new LocationProxy()
                {
                    Id = user.TownId.HasValue ? user.TownId.Value : (int)enTown.Sofia
                };
            }

            if (hold.Team == null || hold.Team.Id == 0)
            {
                hold.Team = new TeamProxy() { Id = DMC.Config.DefaultTeamId };
            }

            _deviceService.SubmitToHolder(id, user.Id, hold.Team.Id, hold.IsBusy, hold.Location.Town);

            return new ResponseMessage()
            {
                Message = DMC.Common.SuccessMessage
            };
        }

        [HttpGet]
        public DeviceHistoryProxyList History(int id, [FromUri] HistoryFilter filter)
        {
            int maxItems = filter.Limit ?? DMC.Common.DefaultPageSize;
            int itemsPerPage = filter.Offset ?? 0;

            DeviceHistoryProxyList historyList = _historyService.GetByDeviceId(id, itemsPerPage, maxItems);
            historyList.Message = DMC.Common.SuccessMessage;
            return historyList;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Save(DeviceProxy simpleDevice)
        {
            bool isSuccessfull = false;

            if (simpleDevice.Id == 0)
            {
                isSuccessfull = _deviceService.Add(simpleDevice);
            }
            else
            {
                isSuccessfull = _deviceService.Edit(simpleDevice);
            }

            ResponseMessage responseMessage = new ResponseMessage();

            if (isSuccessfull)
            {
                return new ResponseMessage()
                {
                    Message = DMC.Common.SuccessMessage
                };
            }
            else if (simpleDevice.Id == 0)
            {
                throw new Exception("An error occured. The device can not be added. Please try again.");
            }
            else
            {
                throw new ArgumentException(string.Format("Device with ID {0} does not exists.", simpleDevice.Id));
            }
        }

        #endregion
    }
}
