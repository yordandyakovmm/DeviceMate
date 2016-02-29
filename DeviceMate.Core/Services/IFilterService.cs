using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IFilterService
    {
        FilterOptions GetDeviceFilterOptions();

        FilterOptions GetAccessoryFilterOptions();

        FilterOptions GetAllDeviceFilterOptions();

        FilterOptions GetAllAccessoryFilterOptions();

        FilterOptions GetUserFilterOptions();
    }
}
