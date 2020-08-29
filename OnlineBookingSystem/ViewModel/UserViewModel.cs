using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [System.Web.Mvc.Compare("RePassWord", ErrorMessage = "Password and confirmation Re-enter Password do not match.")]
        [Required(ErrorMessage = "Password is required.")]
        public string PassWord { get; set; }
        [System.Web.Mvc.Compare("PassWord", ErrorMessage = "Password and confirmation Re-enter Password do not match.")]
        [Display(Name = "Re-enter Password")]
        public string RePassWord { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string Email { get; set; }
        public List<SelectListItem> ListOfRole { get; set; }
        [Display(Name = "Enter Name:")]
        public string NameSearch { get; set; }
    }
}