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
    public class AccessoriesController : ApiController
    {
        #region Fields
        private readonly IAccessoryService _accessoryService;
        private readonly IUserService _userService;
        private readonly IAccessoryHistoryService _historyService;
        #endregion

        #region Constructor
        public AccessoriesController(
            IAccessoryService accessoryService,
            IUserService userService,
            IAccessoryHistoryService hisroyService
            )
            : base()
        {
            _accessoryService = accessoryService;
            _userService = userService;
            _historyService = hisroyService;
        }
        #endregion

        #region HTTP Actions

        [HttpGet]
        public AccessoryProxyList List([FromUri] AccessoryFilter filter)
        {
            filter.Keywords = QueryParamsHelper.GetKeywords(filter.Keyword);
            filter.Sort = QueryParamsHelper.GetSort(filter.SortColumns);

            if (!filter.Offset.HasValue)
            {
                filter.Offset = 0;
            }

            AccessoryProxyList accessoryList = _accessoryService.GetByFilter(filter);
            accessoryList.Message = DMC.Common.SuccessMessage;
            return accessoryList;
        }

        [HttpGet]
        public AccessoryProxy Show(int id)
        {
            AccessoryProxy accessory = _accessoryService.GetById(id);
            accessory.Message = DMC.Common.SuccessMessage;
            return accessory;
        }

        [HttpGet]
        public AccessoryHistoryProxyList History(int id, [FromUri] HistoryFilter filter)
        {
            int maxItems = filter.Limit ?? DMC.Common.DefaultPageSize;
            int itemsPerPage = filter.Offset ?? 0;

            AccessoryHistoryProxyList historyList = _historyService.GetByAccessoryId(id, itemsPerPage, maxItems);
            historyList.Message = DMC.Common.SuccessMessage;
            return historyList;
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Remove(int id)
        {
            _accessoryService.Remove(id);

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

            _accessoryService.SubmitToHolder(id, user.Id, hold.Team.Id, hold.IsBusy, hold.Location.Town);

            return new ResponseMessage()
            {
                Message = DMC.Common.SuccessMessage
            };
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Add(AccessoryProxy simpleAccessory)
        {
            bool isSuccessfull = false;

            if (simpleAccessory.Id == 0)
            {
                isSuccessfull = _accessoryService.Add(simpleAccessory);
            }
            else
            {
                isSuccessfull = _accessoryService.Edit(simpleAccessory);
            }

            ResponseMessage responseMessage = new ResponseMessage();

            if (isSuccessfull)
            {
                return new ResponseMessage()
                {
                    Message = DMC.Common.SuccessMessage
                };
            }
            else if (simpleAccessory.Id == 0)
            {
                throw new Exception("An error occured. The accessory can not be added. Please try again.");
            }
            else
            {
                throw new ArgumentException(string.Format("Accessory with ID {0} does not exists.", simpleAccessory.Id));
            }
        }

        #endregion
    }
}
