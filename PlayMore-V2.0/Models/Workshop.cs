//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlayMore_V5._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Workshop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Workshop()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public int WorkshopId { get; set; }
        public System.DateTime WorkshopDate { get; set; }

        [Required(ErrorMessage = "Please Enter location")]
        [Display(Name = "Workshop Location")]
        public string WorkshopLocation { get; set; }

        [Required(ErrorMessage = "Please Enter location lattitude")]
        [Display(Name = "Lattitude of location")]
        public string WSLocLattitude { get; set; }

        [Required(ErrorMessage = "Please Enter location longitude")]
        [Display(Name = "Longitude of location")]
        public string WSLocLongitude { get; set; }

        [Required(ErrorMessage = "Please Enter Workshop fees")]
        [Display(Name = "Fees of workshop")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage ="Enter only numbers for Price")]
        public string WorkShopFees { get; set; }

        [Display(Name = "Coach Name")]
        public int CoachCoachId { get; set; }

        [Display(Name ="Game Name")]
        public int GameGameId { get; set; }
    
        public virtual Coach Coach { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Game Game { get; set; }
    }
}
