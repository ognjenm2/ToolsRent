//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolsRent.Dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservations()
        {
            this.ToolsReservations = new HashSet<ToolsReservations>();
        }
    
        public int ReservationID { get; set; }
        public string ImePrezime { get; set; }
        public Nullable<System.DateTime> OfferDateTime { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> PriceAll { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToolsReservations> ToolsReservations { get; set; }
    }
}
