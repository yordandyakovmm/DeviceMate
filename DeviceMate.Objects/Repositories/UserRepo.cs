using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class UserRepo : BaseRepo<User>
    {
        [Microsoft.Practices.Unity.InjectionConstructor]
        public UserRepo(DeviceContext context)
            : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return Context.Users.SingleOrDefault(x => x.Email == email);
        }
    }
}
