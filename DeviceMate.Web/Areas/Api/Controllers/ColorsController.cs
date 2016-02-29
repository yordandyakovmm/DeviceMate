using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class ColorsController : ApiController
    {
        #region Fields
        private readonly IColorService _colorService;
        #endregion

        #region Constructor
        public ColorsController(
            IColorService colorService
            )
            : base()
        {
            _colorService = colorService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ColorProxy colorProxy)
        {
            if (colorProxy.Id == 0)
            {
                _colorService.Add(colorProxy);
            }
            else
            {
                _colorService.Edit(colorProxy);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _colorService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
