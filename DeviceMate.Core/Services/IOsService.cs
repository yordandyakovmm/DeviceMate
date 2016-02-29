using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IOsService
    {
        int GetIdByName(string name);

        bool Add(Platform platform);

        bool Edit(Platform platform);

        bool Delete(int id); 
    }
}
