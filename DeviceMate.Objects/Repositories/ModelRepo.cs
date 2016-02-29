using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class ModelRepo : BaseRepo<Model>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ModelRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public IEnumerable<Model> GetAll()
        {
            return this.Context.Models.OrderBy(model => model.Name);
        }

        public IEnumerable<Model> GetByManufacturerId(int id)
        {
            var result = this.Context.Models.Where(m => m.ManufacturerId == id).OrderBy(model => model.Name);
            return result;
        }
    }
}
