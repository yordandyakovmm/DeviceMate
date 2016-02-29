using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class GridColumnRepo : BaseRepo<GridColumn>
    {
        [Microsoft.Practices.Unity.InjectionConstructor]
        public GridColumnRepo(DeviceContext context)
            : base(context)
        {
        }

        public IList<GridColumn> GetByGridId(int gridId)
        {
            return Context.GridColumns.Where(x => x.GridId == gridId).ToList();
        }
    }
}
