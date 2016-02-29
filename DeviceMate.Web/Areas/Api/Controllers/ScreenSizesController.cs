using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class ScreenSizesController : ApiController
    {
        #region Fields
        private readonly IScreenSizeService _screenSizeService;
        #endregion

        #region Constructor
        public ScreenSizesController(
            IScreenSizeService screenSizeService
            )
            : base()
        {
            _screenSizeService = screenSizeService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ScreenSizeProxy screenSize)
        {
            if (screenSize.Id == 0)
            {
                _screenSizeService.Add(screenSize);
            }
            else
            {
                _screenSizeService.Edit(screenSize);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _screenSizeService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
