using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class GridRepo : BaseRepo<Grid>
    {
        [Microsoft.Practices.Unity.InjectionConstructor]
        public GridRepo(DeviceContext context)
            : base(context)
        {
        }

        public Grid GetByName(string name)
        {
            return Context.Grids.FirstOrDefault(x => x.Name == name);
        }
    }
}
