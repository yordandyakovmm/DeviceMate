using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class AccessoryDescriptionRepo : BaseRepo<AccessoryDescription>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public AccessoryDescriptionRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public IEnumerable<AccessoryDescription> GetAll()
        {
            IQueryable<AccessoryDescription> descriptions =
                this.Context.AccessoryDescriptions.OrderBy(description => description.Description);

            return descriptions;
        }

        #endregion
    }
}
