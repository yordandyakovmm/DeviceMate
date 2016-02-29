using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class Sorter
    {
        public enum SortDirection
        {
            None = 0,
            Ascending = 1,
            Descending = -1
        }

        public string Column { get; set; }
        public SortDirection Direction { get; set; }

        public string Expression { get; set; }
    }
}