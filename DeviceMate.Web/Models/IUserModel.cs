using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Web.Models
{
    public interface IUserModel
    {
        bool IsAdmin { get; set; }
        string UserName { get; set; }
    }
}
