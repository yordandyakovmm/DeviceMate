using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class AccessoryTypesController : ApiController
    {
        #region Fields
        private readonly IAccessoryTypeService _accessoryTypeService;
        #endregion

        #region Constructor
        public AccessoryTypesController(
            IAccessoryTypeService accessoryTypeService
            )
            : base()
        {
            _accessoryTypeService = accessoryTypeService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(AccessoryTypeProxy accessoryTypeProxy)
        {
            if (accessoryTypeProxy.Id == 0)
            {
                _accessoryTypeService.Add(accessoryTypeProxy);
            }
            else
            {
                _accessoryTypeService.Edit(accessoryTypeProxy);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _accessoryTypeService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
