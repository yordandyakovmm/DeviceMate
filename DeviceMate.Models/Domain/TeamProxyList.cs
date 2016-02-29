using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class TeamProxyList : PagedItems, IResponseMessage
    {
        public IList<TeamProxy> Teams { get; set; }

        public string Message { get; set; }
    }
}
