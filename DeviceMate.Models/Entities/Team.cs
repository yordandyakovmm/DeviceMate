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
    
    public partial class Team
    {
        public Team()
        {
            this.AccessoryHoldsHistories = new HashSet<AccessoryHoldsHistory>();
            this.DeviceHoldsHistories = new HashSet<DeviceHoldsHistory>();
            this.Holds = new HashSet<Hold>();
            this.Users = new HashSet<User>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<AccessoryHoldsHistory> AccessoryHoldsHistories { get; set; }
        public virtual ICollection<DeviceHoldsHistory> DeviceHoldsHistories { get; set; }
        public virtual ICollection<Hold> Holds { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
