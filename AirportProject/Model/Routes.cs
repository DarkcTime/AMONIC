//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirportProject.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Routes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Routes()
        {
            this.Schedules = new HashSet<Schedules>();
        }
    
        public int ID { get; set; }
        public int DepartureAirportID { get; set; }
        public int ArrivalAirportID { get; set; }
        public int Distance { get; set; }
        public int FlightTime { get; set; }
    
        public virtual Airports Airports { get; set; }
        public virtual Airports Airports1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedules> Schedules { get; set; }
    }
}
