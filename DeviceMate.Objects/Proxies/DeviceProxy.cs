using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class DeviceProxy
    {
        public ModelProxy Model { get; set; }

        public int? Id { get; set; }

        [Required(ErrorMessage = "The number field is required.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        public string Name { get; set; }

        public Nullable<int> ColorId { get; set; }

        [Required(ErrorMessage = "You should select model.")]
        public int? ModelId { get; set; }

        [Required(ErrorMessage = "You should select device type.")]
        public int DeviceTypeId{ get; set ;}

        [MaxLength(50, ErrorMessage = "Additional Info must be no more than 50 characters long.")]
        public string SerialNumber { get; set; }

        [RegularExpression("^[0-9]{1,3}\\.?[0-9]{0,3}\\.?[0-9]{0,3}$", ErrorMessage="Invalid version")]
        public string OSVersion { get; set; }

        [Required(ErrorMessage = "You should select OS.")]
        public int? OsId { get; set; }

        [Required(ErrorMessage = "You should select manufacturer.")]
        public int? ManufacturerId { get; set; }

        [Required(ErrorMessage = "You should select screen size.")]
        public int? ScreenSizeId { get; set; }

        [Required(ErrorMessage = "You should select resolution width.")]
        public int? ResolutionWidthId { get; set; }

        [Required(ErrorMessage = "You should select resolution height.")]
        public int? ResolutionHeightId { get; set; }

        public int HoldId { get; set; }

        [Required(ErrorMessage = "You should select town.")]
        public int? TownId { get; set; }
    }
}
