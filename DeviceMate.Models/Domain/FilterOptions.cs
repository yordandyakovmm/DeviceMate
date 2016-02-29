using System.Collections.Generic;
using DeviceMate.Models.Domain.Interfaces;

namespace DeviceMate.Models.Domain
{
    public class FilterOptions: IResponseMessage
    {
        //Common filter options
        public IList<LocationProxy> Cities { get; set; }
        public IList<TeamProxy> Teams { get; set; }

        //Device and accessory filter options
        public IList<Platform> Platforms { get; set; }
        public IList<ColorProxy> Colors { get; set; }

        //Device filter options
        public IList<DeviceTypeProxy> DeviceType { get; set; }
        public IList<ResolutionProxy> Resolution { get; set; }
        public IList<ScreenSizeProxy> ScreenSize { get; set; }
        public IList<ResolutionDimention> ScreenWidth { get; set; }
        public IList<ResolutionDimention> ScreenHeight { get; set; }

        //Accessory filter options
        public IList<AccessoryDescriptionProxy> Descriptions { get; set; }
        public IList<AccessoryTypeProxy> AccessoryType { get; set; }

        public string Message { get; set; }
    }
}
