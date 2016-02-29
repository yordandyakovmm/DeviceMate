using DeviceMate.Models.Entities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class StatusRepo : BaseRepo<Status>
    {
        #region Construction
        [InjectionConstructor]
        public StatusRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public virtual IEnumerable<Status> GetAll()
        {
            return this.Context.Status.OrderBy(d => d.Name);
        }
    }
}
