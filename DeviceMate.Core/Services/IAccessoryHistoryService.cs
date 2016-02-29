using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IAccessoryHistoryService
    {
        AccessoryHistoryProxyList GetByAccessoryId(int accessoryId, int itemsPerPage, int maxItems);
    }
}
