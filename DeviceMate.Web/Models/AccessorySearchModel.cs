using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class AccessorySearchModel
    {
        public string Number { get; set; }
        public string SerialNumber { get; set; }
        public int? TypeId { get; set; }
        public int? DescriptionId { get; set; }
        public string Email { get; set; }
        public int? TeamId { get; set; }
        public int? TownId { get; set; }
        public int? OsId { get; set; }
        public int? ColorId { get; set; }
    }
}