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
    
    public partial class AmenitiesTickets
    {
        public int ID { get; set; }
        public int AmenityID { get; set; }
        public int TicketID { get; set; }
        public decimal Price { get; set; }
    
        public virtual Amenities Amenities { get; set; }
        public virtual Tickets Tickets { get; set; }
    }
}
