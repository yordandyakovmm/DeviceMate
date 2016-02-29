using System;

namespace DeviceMate.Models.Domain.Abstract
{
    public abstract class PagerFilter
    {
        public Nullable<int> Offset { get; set; }

        public Nullable<int> Limit { get; set; }
    }
}
