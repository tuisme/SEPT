using OnlineBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookingSystem.ViewModel
{
    public class BookingViewModel
    {
        [Display(Name = "Room")]
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Date")]
        public DateTime BookingDate { get; set; }
        [Display(Name = "Booking Hours")]
        public int BookingHourID { get; set; }
        public int RoomCapacity { get; set; }
        public decimal RoomPrice { get; set; }
        public string RoomDescription { get; set; }
        [Display(Name = "Number Of Members")]
        [Required(ErrorMessage = "Number of members is required.")]
        [Range(1, 99999999, ErrorMessage = "Number of members should be equal and greater than {1}")]
        public string NumberOfMembers { get; set; }
        public List<SelectListItem> ListBookingHours { get; set; }
        public List<SelectListItem> RoomBookeds { get; set; }
        public IEnumerable<SelectListItem> ListOfRooms { get; set; }
        public int RoomTypeSearchId { get; set; }
        public List<SelectListItem> ListOfRoomTypeSearch { get; set; }
        public int RoomSearchId { get; set; }
        public List<SelectListItem> ListOfRoomSearch { get; set; }
    }
}