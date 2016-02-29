using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IAccessoryDescriptionService
    {
        bool Add(AccessoryDescriptionProxy accessoryDescriptionProxy);

        bool Edit(AccessoryDescriptionProxy accessoryDescriptionProxy);

        bool Delete(int id);
    }
}
