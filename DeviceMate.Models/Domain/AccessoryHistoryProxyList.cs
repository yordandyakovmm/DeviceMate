using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class AccessoryHistoryProxyList : PagedItems, IResponseMessage
    {
        public IList<AccessoryHistoryProxy> History { get; set; }
        public string Message { get; set; }
    }
}
