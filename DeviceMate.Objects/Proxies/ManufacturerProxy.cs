using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class ManufacturerProxy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public int OsId { get; set; }
    }
}
