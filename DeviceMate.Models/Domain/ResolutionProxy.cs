using DeviceMate.Models.Domain.Abstract;

namespace DeviceMate.Models.Domain
{
    public class ResolutionProxy : IdNameModel
    {
        public ResolutionDimention Width { get; set; }
        public ResolutionDimention Height { get; set; }
    }
}
