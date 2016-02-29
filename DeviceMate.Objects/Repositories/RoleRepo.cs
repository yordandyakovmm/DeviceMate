using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class RoleRepo : BaseRepo<Role>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public RoleRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public virtual bool RoleExists(string roleName)
        {
            return Context.Roles.Any(r => r.Name == roleName);
        }

        public virtual IEnumerable<Role> GetAll()
        {
            return Context.Roles.OrderBy(r => r.Id);
        }

        #endregion
    }
}
