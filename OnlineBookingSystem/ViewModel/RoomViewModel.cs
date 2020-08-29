using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace OnlineBookingSystem.ViewModel
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Room No.")]
        [Required(ErrorMessage ="Room number is required.")]
        public string RoomNumber { get; set; }
        [Display(Name = "Room Price")]
        [Range(0, 99999999, ErrorMessage = "Room price should be equal and greater than {1}")]
        [Required(ErrorMessage = "Room Price is required.")]
        public decimal RoomPrice { get; set; }
        [Display(Name = "Room Type")]
        public int RoomTypeId { get; set; }
        [Display(Name = "Room Capacity")]
        [Range(1, 99999999, ErrorMessage = "Room Capacity should be equal and greater than {1}")]
        public int RoomCapacity { get; set; }
        [Display(Name = "Room Description")]
        public string RoomDescription { get; set; }
        [Display(Name = "Students Not Allowed")]
        public bool StudentsNotAllowed { get; set; }
        public List<SelectListItem> ListOfRoomType { get; set; }
        [Display(Name = "Type")]
        public int RoomTypeSearchId { get; set; }
        public List<SelectListItem> ListOfRoomTypeSearch { get; set; }
    }
}