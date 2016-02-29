
namespace DeviceMate.Models.Domain
{
    public class DeviceHistoryProxy
    {
        public int Id { get; set; }
        public DeviceProxy Device { get; set; }
        public HoldProxy Hold { get; set; }
    }
}
