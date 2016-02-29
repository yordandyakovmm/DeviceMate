using DeviceMate.Models.Domain.Abstract;
using DeviceMate.Models.Domain.Interfaces;
using System;

namespace DeviceMate.Models.Domain
{
    public class AccessoryProxy : IdNameModel, IResponseMessage
    {
        public string Info { get; set; }

        public DateTime DateTaken { get; set; }

        public AccessoryTypeProxy Type { get; set; }

        public AccessoryDescriptionProxy Description { get; set; }

        public HoldProxy Holder { get; set; }

        public Platform Platform { get; set; }

        public ColorProxy Color { get; set; }

        public string Message { get; set; }
    }
}
