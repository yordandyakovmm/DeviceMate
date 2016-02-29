using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize(Roles = DMC.Common.AdminUserRole)]
    public class OsController : ApiController
    {
        #region Fields
        private readonly IOsService _osService;
        #endregion

        #region Constructor
        public OsController(
            IOsService osService
            )
            : base()
        {
            _osService = osService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(Platform platform)
        {
            if (platform.Id == 0)
            {
                _osService.Add(platform);
            }
            else
            {
                _osService.Edit(platform);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _osService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
