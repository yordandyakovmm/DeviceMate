using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class AccessoryDescriptionsController : ApiController
    {
        #region Fields
        private readonly IAccessoryDescriptionService _accessoryDescriptionService;
        #endregion

        #region Constructor
        public AccessoryDescriptionsController(
            IAccessoryDescriptionService accessoryDescriptionService
            )
            : base()
        {
            _accessoryDescriptionService = accessoryDescriptionService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(AccessoryDescriptionProxy accessoryDescriptionProxy)
        {
            if (accessoryDescriptionProxy.Id == 0)
            {
                _accessoryDescriptionService.Add(accessoryDescriptionProxy);
            }
            else
            {
                _accessoryDescriptionService.Edit(accessoryDescriptionProxy);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _accessoryDescriptionService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
