using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class AccessoryProxy
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "You should select an accessory type.")]
        public int AccessoryTypeId { get; set; }

        [Required(ErrorMessage = "You should select an accessory description.")]
        public int AccessoryDescriptionId { get; set; }

        [Required(ErrorMessage = "The number field is required.")]
        public string Number { get; set; }

        public string SerialNumber { get; set; }

        public int OsId { get; set; }

        public int ColorId { get; set; }
    }
}
