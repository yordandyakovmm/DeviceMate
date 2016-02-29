using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Enums;
using System;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class DeviceFilter : PagerFilter
    {
        public Nullable<int> TownId { get; set; }
        
        public Nullable<int> OsId { get; set; }

        public Nullable<int> ManufacturerId { get; set; }

        public Nullable<int> ModelId { get; set; }

        public Nullable<int> TeamId { get; set; }

        public Nullable<int> ScreenSizeId { get; set; }

        public Nullable<int> ResolutionWidthId { get; set; }

        public Nullable<int> ResolutionHeightId { get; set; }

        public Nullable<int> ColorId { get; set; }

        public Nullable<int> TypeId { get; set; }

        public Nullable<bool> IsAvailable { get; set; }

        //The data that will be get from the query string
        public string SortColumns { get; set; }

        //The sort data that will be processed
        public Dictionary<enSortColumn, enSortOrder> Sort { get; set; }

        //The data that will be get from the query string
        public string Keyword { get; set; }

        //The keywords data that will be processed
        public string[] Keywords { get; set; }
    }
}
