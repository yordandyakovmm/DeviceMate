//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeviceMate.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Device
    {
        public Device()
        {
            this.DeviceHoldsHistories = new HashSet<DeviceHoldsHistory>();
        }
    
        public int Id { get; set; }
        public string Number { get; set; }
        public Nullable<int> ColorId { get; set; }
        public string OsVersion { get; set; }
        public int HoldId { get; set; }
        public int ModelId { get; set; }
        public int DeviceTypeId { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public Nullable<int> ResolutionId { get; set; }
        public Nullable<int> ScreenSizeId { get; set; }
    
        public virtual Color Color { get; set; }
        public virtual ICollection<DeviceHoldsHistory> DeviceHoldsHistories { get; set; }
        public virtual Hold Hold { get; set; }
        public virtual DeviceType DeviceType { get; set; }
        public virtual Model Model { get; set; }
        public virtual Resolution Resolution { get; set; }
        public virtual ScreenSize ScreenSize { get; set; }
    }
}
