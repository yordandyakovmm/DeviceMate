using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class ScreenSizeProxy
    {
        public int? Id { get; set; }

        [Range(0, 20, ErrorMessage = "Screen size should not be more than 20''.")]
        public decimal Size { get; set; }
    }
}
