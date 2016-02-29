using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    public class ModelsController : ApiController
    {
        #region Fields
        private readonly IModelService _modelService;
        #endregion

        #region Constructor
        public ModelsController(
            IModelService modelService
            )
            : base()
        {
            _modelService = modelService;
        }
        #endregion

        #region HTTP Actions
        [HttpPost]
        public ResponseMessage Save(ModelProxy modelProxy)
        {
            if (modelProxy.Id == 0)
            {
                _modelService.Add(modelProxy);
            }
            else
            {
                _modelService.Edit(modelProxy);
            }

            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        [HttpGet]
        public ResponseMessage Delete(int id)
        {
            _modelService.Delete(id);
            return new ResponseMessage() { Message = DMC.Common.SuccessMessage };
        }

        #endregion
    }
}
