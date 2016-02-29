using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class ResolutionHeightRepo : BaseRepo<ResolutionHeightOption>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ResolutionHeightRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public IEnumerable<ResolutionHeightOption> GetAll()
        {
            return this.Context.ResolutionHeightOptions.OrderBy(x => x.Height);
        }

        public int Delete(int id)
        {
            var height = this.Context.ResolutionHeightOptions.Find(id);
            return height == null ? 0 : base.Delete(height);
        }
    }
}
