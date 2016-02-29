using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class HomeModel : IUserModel
    {
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
    }
}