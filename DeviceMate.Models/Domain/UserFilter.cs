using DeviceMate.Models.Enums;
using System;
using System.Collections.Generic;
using DeviceMate.Models.Domain.Abstract;

namespace DeviceMate.Models.Domain
{
    public class UserFilter : PagerFilter
    {
        public Nullable<int> TownId { get; set; }

        public Nullable<int> TeamId { get; set; }

        public Nullable<bool> IsAdmin { get; set; }

        public Nullable<enUserStatus> StatusId { get; set; }        

        public string SortColumns { get; set; }

        public Dictionary<enSortColumn, enSortOrder> Sort { get; set; }

        public string Keyword { get; set; }

        public string[] Keywords { get; set; }
    }
}