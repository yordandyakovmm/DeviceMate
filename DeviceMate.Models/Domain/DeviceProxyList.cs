using System;
using System.Collections.Generic;
using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;

namespace DeviceMate.Models.Domain
{
    public class DeviceProxyList : PagedItems
    {
        public IList<DeviceProxy> Devices { get; set; }
        public int AvailableCount { get; set; }
        public string Message { get; set; }
    }
}
