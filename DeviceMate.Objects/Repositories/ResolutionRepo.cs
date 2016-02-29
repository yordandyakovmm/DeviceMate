using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class ResolutionRepo : BaseRepo<Resolution>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ResolutionRepo(DeviceContext context) : base(context)
        {
        }
        #endregion

        public Resolution GetByWidthIdHeightId(int widthId, int heightId)
        {
            return
                this.Context.Resolutions.FirstOrDefault(r => r.ResolutionWidthId == widthId && r.ResolutionHeightId == heightId);
        }
    }
}
