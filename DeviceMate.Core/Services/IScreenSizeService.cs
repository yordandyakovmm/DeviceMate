using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IScreenSizeService
    {
        bool Add(ScreenSizeProxy screenSize);

        bool Edit(ScreenSizeProxy screenSize);

        bool Delete(int id); 
    }
}
