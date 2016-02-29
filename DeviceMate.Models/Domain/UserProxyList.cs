using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class UserProxyList : PagedItems, IResponseMessage
    {
        public IList<UserProxy> Users { get; set; }

        public string Message { get; set; }
    }
}
