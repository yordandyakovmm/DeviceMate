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
    
    public partial class ResolutionHeightOption
    {
        public ResolutionHeightOption()
        {
            this.Resolutions = new HashSet<Resolution>();
        }
    
        public int Id { get; set; }
        public int Height { get; set; }
    
        public virtual ICollection<Resolution> Resolutions { get; set; }
    }
}
