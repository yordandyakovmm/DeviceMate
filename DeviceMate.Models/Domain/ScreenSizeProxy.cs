using DeviceMate.Models.Domain.Abstract;
using System.Globalization;

namespace DeviceMate.Models.Domain
{
    public class ScreenSizeProxy : IdNameModel
    {
        public override string Name
        {
            get
            {
                return string.Format("{0}\"", Size.ToString("#.##", CultureInfo.InvariantCulture));
            }
        }
        public decimal Size { get; set; }
    }
}
