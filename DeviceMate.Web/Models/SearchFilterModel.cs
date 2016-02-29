using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class SearchFilterModel
    {
        public string Number { get; set; }
        public IEnumerable<int> ModelId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? OsId { get; set; }
        public string Name { get; set; }
        public int? ColorId { get; set; }
        public int? TeamId { get; set; }
        public string SerialNumber { get; set; }
        public string Email { get; set; }
        public string OSVersion { get; set; }
        public int? TypeId { get; set; }
        public int? AvailableID { get; set; }
        public int? TownID { get; set; }
        public int? ScreenSizeId { get; set; }
        public int? ResolutionId { get; set; }
        public int? ResolutionWidthId { get; set; }
        public int? ResolutionHeightId { get; set; }
    }
}