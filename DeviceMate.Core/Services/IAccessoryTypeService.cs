using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IAccessoryTypeService
    {
        bool Add(AccessoryTypeProxy accessoryTypeProxy);

        bool Edit(AccessoryTypeProxy accessoryTypeProxy);

        bool Delete(int id);
    }
}