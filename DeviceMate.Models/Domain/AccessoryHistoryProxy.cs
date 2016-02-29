
namespace DeviceMate.Models.Domain
{
    public class AccessoryHistoryProxy
    {
        public int Id { get; set; }
        public AccessoryProxy Accessory { get; set; }
        public HoldProxy Hold { get; set; }
    }
}
