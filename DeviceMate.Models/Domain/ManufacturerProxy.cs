using DeviceMate.Models.Domain.Abstract;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class ManufacturerProxy : IdNameModel
    {
        public int OsId { get; set; }
        public bool IsRemovable { get; set; }
        public IList<ModelProxy> Models { get; set; }
    }
}
