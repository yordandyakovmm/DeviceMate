using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;

namespace DeviceMate.Objects.Repositories
{
    public class ColorRepo : BaseRepo<Color>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public ColorRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public List<Color> GetAll()
        {
            return this.Context.Colors.OrderBy(d => d.Name).ToList();
        }
    }
}
