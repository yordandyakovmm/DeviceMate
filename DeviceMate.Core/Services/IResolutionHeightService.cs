using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IResolutionHeightService
    {
        bool Add(ResolutionDimention heightDimention);

        bool Edit(ResolutionDimention heightDimention);

        bool Delete(int id);
    }
}
