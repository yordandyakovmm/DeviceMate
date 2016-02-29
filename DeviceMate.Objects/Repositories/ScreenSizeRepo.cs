using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class ScreenSizeRepo : BaseRepo<ScreenSize>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ScreenSizeRepo(DeviceContext context) : base(context)
        {
        }
        #endregion

        public IEnumerable<ScreenSize> GetAll()
        {
            return this.Context.ScreenSizes.OrderBy(x => x.Size);
        }
    }
}
