using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Web.Areas.Api.Helpers;
using System;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize]
    public class TeamsController : ApiController
    {
        #region Fields
        private readonly ITeamService _teamService;
        #endregion

        #region Constructor
        public TeamsController(ITeamService teamService)
            : base()
        {
            _teamService = teamService;
        }
        #endregion

        #region HTTP Methods

        [HttpGet]
        public TeamProxyList List([FromUri] TeamFilter filter)
        {
            filter.Keywords = QueryParamsHelper.GetKeywords(filter.Keyword);
            filter.Sort = QueryParamsHelper.GetSort(filter.SortColumns);

            TeamProxyList teamList = _teamService.GetByFilter(filter);
            teamList.Message = DMC.Common.SuccessMessage;
            return teamList;
        }

        [HttpGet]
        public TeamProxy Show(int id)
        {
            TeamProxy team = _teamService.GetById(id);
            team.Message = DMC.Common.SuccessMessage;
            return team;
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Remove(int id)
        {
            _teamService.Delete(id);

            return new ResponseMessage()
            {
                Message = DMC.Common.SuccessMessage
            };
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ResponseMessage Add(TeamProxy simpleTeam)
        {
            bool isSuccessfull = false;

            if (simpleTeam.Id == 0)
            {
                isSuccessfull = _teamService.Add(simpleTeam);
            }
            else
            {
                isSuccessfull = _teamService.Edit(simpleTeam);
            }

            ResponseMessage responseMessage = new ResponseMessage();

            if (isSuccessfull)
            {
                return new ResponseMessage()
                {
                    Message = DMC.Common.SuccessMessage
                };
            }
            else if (simpleTeam.Id == 0)
            {
                throw new Exception("An error occured. The team can not be added. Please try again.");
            }
            else
            {
                throw new ArgumentException(string.Format("Team with ID {0} does not exists.", simpleTeam.Id));
            }
        }

        #endregion
    }
}
