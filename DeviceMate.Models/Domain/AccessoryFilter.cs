using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Enums;
using System;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class AccessoryFilter : PagerFilter
    {
        public Nullable<int> DescriptionId { get; set; }

        public Nullable<int> TownId { get; set; }

        public Nullable<int> OsId { get; set; }

        public Nullable<int> TeamId { get; set; }

        public Nullable<int> ColorId { get; set; }

        public Nullable<int> TypeId { get; set; }

        public Nullable<bool> IsAvailable { get; set; }

        public string SortColumns { get; set; }

        public Dictionary<enSortColumn, enSortOrder> Sort { get; set; }

        public string Keyword { get; set; }

        public string[] Keywords { get; set; }
    }
}
