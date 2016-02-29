using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Web.Areas.Api.Helpers;
using System.Threading.Tasks;
using System.Web.Http;
using DMC = DeviceMate.Core;
using DeviceMate.Models.Enums;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        #endregion

        #region Constructor
        public UsersController(IUserService userService, ITeamService teamService)
        {
            _userService = userService;
            _teamService = teamService;
        }
        #endregion

        #region HTTP Actions
        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public async Task<ResponseMessage> UpdateAll()
        {
            ResponseMessage response = new ResponseMessage();
            if (await _userService.UpdateFromEmplyeeData(ignoreTimeout: true, createMissingUsers: true))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = "An error occured. Please try again.";
            }

            return response;
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        [Route("api/v1/users/update/{id}")]
        public async Task<ResponseMessage> Update(int id)
        {
            ResponseMessage response = new ResponseMessage();

            var user = _userService.GetById(id);

            if (user != null && await _userService.UpdateFromEmplyeeData(user.Email))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = "An error occured. Please try again.";
            }

            return response;
        }

        [HttpGet]
        public UserProxyList List([FromUri] UserFilter filter)
        {

            filter.Keywords = QueryParamsHelper.GetKeywords(filter.Keyword);
            filter.Sort = QueryParamsHelper.GetSort(filter.SortColumns);

            if (!filter.Offset.HasValue)
            {
                filter.Offset = 0;
            }

            UserProxyList userList = _userService.GetByFilter(filter);
            userList.Message = DMC.Common.SuccessMessage;
            return userList;
        }

        [HttpGet]
        public UserProxy Show(int id)
        {
            UserProxy user = _userService.GetUserProxyById(id);
            user.Message = DMC.Common.SuccessMessage;
            return user;
        }

        [HttpGet]
        public UserProxy Show(int id, bool? getHistory)
        {
            UserProxy user = _userService.GetUserProxyById(id, getHistory.Value);
            user.Message = DMC.Common.SuccessMessage;
            return user;
        }


        [HttpGet]
        [Route("api/v1/users/me")]
        public UserProxy GetCurrentUser()
        {
            UserProxy user = _userService.GetUserProxyByEmail(User.Identity.Name);
            user.Message = DMC.Common.SuccessMessage;
            return user;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Deactivate(int id)
        {
            ResponseMessage response = new ResponseMessage();

            if (_userService.SetStatus(id, enUserStatus.Inactive))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = string.Format("User with ID {0} does not exists", id);
            }

            return response;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Activate(int id)
        {
            ResponseMessage response = new ResponseMessage();

            if (_userService.SetStatus(id, enUserStatus.Active))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = string.Format("User with ID {0} does not exists", id);
            }

            return response;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Delete(int id)
        {
            ResponseMessage response = new ResponseMessage();

            if (_userService.Delete(id))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = string.Format("User with ID {0} does not exists", id);
            }

            return response;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage PromoteAdmin(int id, [FromUri] int teamId)
        {
            if (teamId == 0)
            {
                teamId = DMC.Config.DefaultTeamId;
            }

            ResponseMessage response = new ResponseMessage();

            if (_userService.SetAdminStatus(id, true, teamId))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = string.Format("User with ID {0} does not exists", id);
            }

            return response;
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage RevokeAdmin(int id)
        {
            ResponseMessage response = new ResponseMessage();

            if (_userService.SetAdminStatus(id, false))
            {
                response.Message = DMC.Common.SuccessMessage;
            }
            else
            {
                response.Message = string.Format("User with ID {0} does not exists", id);
            }

            return response;
        }

        #endregion
    }
}
