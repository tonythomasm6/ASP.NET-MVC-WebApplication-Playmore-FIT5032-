//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlayMore_V2._0.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int BookId { get; set; }
        public string BookedBy_Userid { get; set; }
        public string Name { get; set; }
        public int WorkshopWorkshopId { get; set; }
    
        public virtual Workshop Workshop { get; set; }
    }
}
