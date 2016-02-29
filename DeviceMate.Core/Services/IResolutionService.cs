using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using System.Collections.Generic;

namespace DeviceMate.Core.Services
{
    public interface IResolutionService
    {
        bool Add(ResolutionProxy screenSize);

        bool Delete(int id);

        void RemoveFromDevicesWithoutSave(IEnumerable<Resolution> resolutions);

        int GetIdByWidthAndHeight(int widthId, int heightId);
    }
}
