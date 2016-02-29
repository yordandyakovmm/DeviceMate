using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System.Collections.Generic;

namespace DeviceMate.Models.Domain
{
    public class UserProxy : IdNameModel, IResponseMessage
    {
        public string Email { get; set; }
        public string Skype { get; set; }
        public string PictureUrl { get; set; }
        public string Position { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public LocationProxy Location { get; set; }
        public TeamProxy Team { get; set; }
        public IList<DeviceProxy> Devices { get; set; }
        public IList<DeviceHistoryProxy> DeviceHistory { get; set; }

        public string Message { get; set; }
    }
}
