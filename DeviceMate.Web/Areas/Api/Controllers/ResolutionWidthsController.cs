using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize(Roles = DMC.Common.AdminUserRole)]
    public class ResolutionWidthsController : ApiController
    {
        #region Fields
        private readonly IResolutionWidthService _resolutionWidthService;
        #endregion

        #region Constructor
        public ResolutionWidthsController(
            IResolutionWidthService resolutionWidthService
            )
            : base()
        {
            _resolutionWidthService = resolutionWidthService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ResolutionDimention width)
        {
            if (width.Id == 0)
            {
                _resolutionWidthService.Add(width);
            }
            else
            {
                _resolutionWidthService.Edit(width);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _resolutionWidthService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
