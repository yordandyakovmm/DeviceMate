using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;

namespace DeviceMate.Objects.Repositories
{
    public class DeviceTypeRepo : BaseRepo<DeviceType>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public DeviceTypeRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public virtual IEnumerable<DeviceType> GetAll()
        {
            return this.Context.DeviceTypes.OrderBy(d => d.Name);
        }
    }
}
