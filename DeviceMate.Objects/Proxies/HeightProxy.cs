using System.ComponentModel.DataAnnotations;

namespace DeviceMate.Objects.Proxies
{
    public class HeightProxy
    {
        public int? Id { get; set; }

        [Range(0, 10000, ErrorMessage = "Device Height should not be more than 10000 px.")]
        public int Height { get; set; }
    }
}
