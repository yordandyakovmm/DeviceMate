using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class ResolutionHeightsController : ApiController
    {
        #region Fields
        private readonly IResolutionHeightService _resolutionHeightService;
        #endregion

        #region Constructor
        public ResolutionHeightsController(
            IResolutionHeightService resolutionHeightService
            )
            : base()
        {
            _resolutionHeightService = resolutionHeightService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ResolutionDimention height)
        {
            if (height.Id == 0)
            {
                _resolutionHeightService.Add(height);
            }
            else
            {
                _resolutionHeightService.Edit(height);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _resolutionHeightService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
