using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class DeviceHistoryProxyList : PagedItems, IResponseMessage
    {
        public IList<DeviceHistoryProxy> History { get; set; }
        public string Message { get; set; }
    }
}
