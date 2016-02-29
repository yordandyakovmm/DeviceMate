using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using System.Web.Http;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Areas.Api.Controllers
{
    [Authorize]
    public class FiltersController : ApiController
    {
        #region Fields
        private readonly IFilterService _filterService;
        #endregion

        #region Constructor
        public FiltersController(IFilterService filterService)
        {
            _filterService = filterService;
        }
        #endregion

        #region HTTP Actions
        [HttpGet]
        public FilterOptions Devices()
        {
            FilterOptions filterOptions = _filterService.GetDeviceFilterOptions();
            filterOptions.Message = DMC.Common.SuccessMessage;
            return filterOptions;
        }

        [HttpGet]
        public FilterOptions Accessories()
        {
            FilterOptions filterOptions = _filterService.GetAccessoryFilterOptions();
            filterOptions.Message = DMC.Common.SuccessMessage;
            return filterOptions;
        }

        [HttpGet]
        [Route("api/v1/filters/devices/all")]
        public FilterOptions AllDevices()
        {
            FilterOptions filterOptions = _filterService.GetAllDeviceFilterOptions();
            filterOptions.Message = DMC.Common.SuccessMessage;
            return filterOptions;
        }

        [HttpGet]
        [Route("api/v1/filters/accessories/all")]
        public FilterOptions AllAccessories()
        {
            FilterOptions filterOptions = _filterService.GetAllAccessoryFilterOptions();
            filterOptions.Message = DMC.Common.SuccessMessage;
            return filterOptions;
        }

        [HttpGet]
        public FilterOptions Users()
        {
            FilterOptions filterOptions = _filterService.GetUserFilterOptions();
            filterOptions.Message = DMC.Common.SuccessMessage;
            return filterOptions;
        }

        #endregion
    }
}
