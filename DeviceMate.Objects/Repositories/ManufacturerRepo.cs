using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class ManufacturerRepo : BaseRepo<Manufacturer>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ManufacturerRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public IEnumerable<Manufacturer> GetAll()
        {
            return this.Context.Manufacturers.OrderBy(m => m.Name);
        }

        public IEnumerable<Manufacturer> GetByOsId(int id)
        {
            var result = this.Context.Manufacturers.Where(m => m.OsId == id).OrderBy(m => m.Name);
            return result;
        }
    }
}
