using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IDeviceHistoryService
    {
        DeviceHistoryProxyList GetByDeviceId(int deviceId, int itemsPerPage, int maxItems);
    }
}
