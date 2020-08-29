using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBookingSystem.ViewModel
{
    public class RoomUsageDetailsViewModel
    {
        public int Id { get; set; }
        public string Room { get; set; }
        public string User { get; set; }
        public System.DateTime BookingDate { get; set; }
        public string BookingHours { get; set; }
        public string NumberOfMembers { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int NumberOfDays { get; set; }
    }
}