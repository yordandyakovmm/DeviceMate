using DeviceMate.Models.Domain;

namespace DeviceMate.Core.Services
{
    public interface IModelService
    {
        bool Add(ModelProxy model);

        bool Edit(ModelProxy model);

        bool Delete(int id); 
    }
}
