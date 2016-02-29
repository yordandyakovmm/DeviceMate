using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Enums;
using System;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class TeamFilter : PagerFilter
    {
        public Nullable<int> TownId { get; set; }

        public bool GetUsers { get; set; }

        public string SortColumns { get; set; }

        public Dictionary<enSortColumn, enSortOrder> Sort { get; set; }

        public string Keyword { get; set; }

        public string[] Keywords { get; set; }
    }
}
