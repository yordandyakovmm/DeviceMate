using DeviceMate.Models.Domain.Abstract;

namespace DeviceMate.Models.Domain
{
    public class ModelProxy : IdNameModel
    {
        public int ManufacturerId { get; set; }
        public bool IsRemovable { get; set; }
    }
}
