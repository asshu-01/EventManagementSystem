using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EventManagementSystem.Entity
{
    public class BookingEntity
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public string UserEmail { get; set; }
        public int EventID { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime BookingDate { get; set; }
    }
}