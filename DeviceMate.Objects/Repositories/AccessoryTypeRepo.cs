using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class AccessoryTypeRepo : BaseRepo<AccessoryType>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public AccessoryTypeRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public IEnumerable<AccessoryType> GetAll()
        {
            IQueryable<AccessoryType> types =
                this.Context.AccessoryTypes.OrderBy(description => description.Name);

            return types;
        }

        #endregion
    }
}
