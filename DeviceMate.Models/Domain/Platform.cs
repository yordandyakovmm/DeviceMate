using DeviceMate.Models.Domain.Abstract;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class Platform : IdNameModel
    {
        public string Version { get; set; }
        public bool IsRemovable { get; set; }
        public IList<ManufacturerProxy> Manufacturers { get; set; }
    }
}
