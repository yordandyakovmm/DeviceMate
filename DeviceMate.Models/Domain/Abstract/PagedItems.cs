
namespace DeviceMate.Models.Domain.Abstract
{
    public abstract class PagedItems
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
