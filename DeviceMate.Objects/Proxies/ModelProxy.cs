using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class ModelProxy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
    }
}
