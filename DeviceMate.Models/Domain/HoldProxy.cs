
namespace DeviceMate.Models.Domain
{
    public class HoldProxy
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string ImagePath { get; set; }
        public bool IsBusy { get; set; }

        public TeamProxy Team { get; set; }
        public LocationProxy Location { get; set; }
    }
}
