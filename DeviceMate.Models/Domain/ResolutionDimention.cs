using DeviceMate.Models.Domain.Abstract;

namespace DeviceMate.Models.Domain
{
    public class ResolutionDimention : IdNameModel
    {
        public override string Name
        {
            get
            {
                return string.Format("{0}px", Dimention.ToString());
            }
        }

        public int Dimention { get; set; }
    }
}
