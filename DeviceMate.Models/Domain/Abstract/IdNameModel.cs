
namespace DeviceMate.Models.Domain.Abstract
{
    public abstract class IdNameModel
    {
        public int Id { get; set; }
        public virtual string Name { get; set; }
    }
}
