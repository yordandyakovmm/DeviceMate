using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class WidthProxy
    {
        public int? Id { get; set; }

        [Range(0, 10000, ErrorMessage = "Device Width should not be more than 10000 px.")]
        public int Width { get; set; }
    }
}
