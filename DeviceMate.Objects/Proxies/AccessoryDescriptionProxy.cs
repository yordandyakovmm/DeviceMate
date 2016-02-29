using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class AccessoryDescriptionProxy
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        [StringLength(128, ErrorMessage = "The max length of the accessory type description is 128 symbols.")]
        public string Description { get; set; }
    }
}
