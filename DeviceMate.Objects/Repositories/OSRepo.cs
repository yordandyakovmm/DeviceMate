using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class OSRepo : BaseRepo<OSs>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public OSRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public IEnumerable<OSs> GetAll()
        {
            return this.Context.OSses.OrderBy(os => os.Name);
        }
    }
}
