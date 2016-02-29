using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class AccessoryTypeProxy
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [StringLength(64, ErrorMessage = "The max length of the accessory type name is 64 symbols.")]
        public string Name { get; set; }
    }
}
