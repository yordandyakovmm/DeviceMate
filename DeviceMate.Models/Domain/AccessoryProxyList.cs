using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class AccessoryProxyList : PagedItems, IResponseMessage
    {
        public IList<AccessoryProxy> Accessories { get; set; }
        public int AvailableCount { get; set; }
        public string Message { get; set; }
    }
}
