using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class UserGridColumnRepo : BaseRepo<UsersGridColumn>
    {
        [Microsoft.Practices.Unity.InjectionConstructor]
        public UserGridColumnRepo(DeviceContext context)
            : base(context)
        {
        }

        public IList<UsersGridColumn> GetByUserIdAndGridId(int userId, int gridId)
        {
            return Context.UsersGridColumns.Where(x => x.UserId == userId && x.GridColumn.GridId == gridId).ToList();
        }
    }
}
