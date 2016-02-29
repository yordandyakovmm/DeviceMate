using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IColorService
    {
        bool Add(ColorProxy color);

        bool Edit(ColorProxy color);

        bool Delete(int id); 
    }
}