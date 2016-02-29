using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System;

namespace DeviceMate.Models.Domain
{
    public class DeviceProxy : IdNameModel, IResponseMessage
    {
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public string Info { get; set; }
        public string DeviceNumber { get; set; }
        public DateTime DateTaken { get; set; }

        public ModelProxy Model { get; set; }
        public ManufacturerProxy Manufacturer { get; set; }
        public HoldProxy Holder { get; set; }
        public ColorProxy Color { get; set; }
        public ScreenSizeProxy ScreenSize { get; set; }
        public ResolutionProxy Resolution { get; set; }
        public DeviceTypeProxy Type { get; set; }
        public Platform Platform { get; set; }

        public string Message { get; set; }
    }
}
