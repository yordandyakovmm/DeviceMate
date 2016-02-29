using System;

namespace DeviceMate.Objects.Proxies
{
    public class DeviceHoldsHistoryProxy
    {

        public int? Id { get; set; }

        public int TeamId { get; set; }

        public DateTime HoldDate { get; set; }

        public string Email { get; set; }

        public bool IsBusy { get; set; }

        public int TownId { get; set; }

        public int? DeviceId { get; set; }
    }
}

