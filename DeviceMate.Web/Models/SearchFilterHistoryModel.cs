using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class SearchFilterHistoryModel
    {


        public string Number { get; set; } // device name
        public string Name { get; set; } // device name
        public int? TeamId { get; set; }
        public string Email { get; set; }
        public int? TownID { get; set; }
        public int? DeviceTypeID { get; set; }
        public string OsVersion { get; set; }
    }
}