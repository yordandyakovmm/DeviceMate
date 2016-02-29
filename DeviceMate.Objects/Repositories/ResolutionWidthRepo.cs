using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class ResolutionWidthRepo : BaseRepo<ResolutionWidthOption>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ResolutionWidthRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public IEnumerable<ResolutionWidthOption> GetAll()
        {
            return this.Context.ResolutionWidthOptions.OrderBy(x => x.Width);
        }

        public int Delete(int id)
        {
            var width = this.Context.ResolutionWidthOptions.Find(id);
            return width == null ? 0 : base.Delete(width);
        }
    }
}
