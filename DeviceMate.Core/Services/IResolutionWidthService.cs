using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IResolutionWidthService
    {
        bool Add(ResolutionDimention widthDimention);

        bool Edit(ResolutionDimention widthDimention);

        bool Delete(int id);
    }
}
