using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IManufacturerService
    {
        bool Add(ManufacturerProxy manufacturer);

        bool Edit(ManufacturerProxy manufacturer);

        bool Delete(int id); 
    }
}
