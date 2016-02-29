using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Enums;

namespace DeviceMate.Models.Domain
{
    public class LocationProxy : IdNameModel
    {
        public enTown Town
        {
            get
            {
                return (enTown)Id;
            }
        }
    }
}
