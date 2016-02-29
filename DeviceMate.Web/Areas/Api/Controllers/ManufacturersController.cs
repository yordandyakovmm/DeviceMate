using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class ManufacturersController : ApiController
    {
        #region Fields
        private readonly IManufacturerService _manufacturerService;
        #endregion

        #region Constructor
        public ManufacturersController(
            IManufacturerService manufacturerService
            )
            : base()
        {
            _manufacturerService = manufacturerService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ManufacturerProxy manufacturerProxy)
        {
            if (manufacturerProxy.Id == 0)
            {
                _manufacturerService.Add(manufacturerProxy);
            }
            else
            {
                _manufacturerService.Edit(manufacturerProxy);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _manufacturerService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
