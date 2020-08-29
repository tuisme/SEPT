using OnlineBookingSystem.Models;
using OnlineBookingSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private BookingDBEntities objBookingDBEntities;
        public AccountController()
        {
            objBookingDBEntities = new BookingDBEntities();
        }
        public ActionResult Index()
        {
            UserViewModel objUserViewModel = new UserViewModel();
            //Load all User for dropdownlist
            objUserViewModel.ListOfRole = (from obj in objBookingDBEntities.Roles
                                               select new SelectListItem()
                                               {
                                                   Text = obj.Name,
                                                   Value = obj.Id.ToString()
                                               }).ToList();
            return View(objUserViewModel);
        }
        [HttpPost]
        public ActionResult Index(UserViewModel objUserViewModel)
        {
            string message = string.Empty;
            if (objUserViewModel.Id == 0)
            {
                //Insert new a User to database
                User obj = new User()
                {
                    UserName = objUserViewModel.UserName,
                    PassWord = objUserViewModel.PassWord,
                    FullName = objUserViewModel.FullName,
                    Email = objUserViewModel.Email,
                    RoleId = objUserViewModel.RoleId,
                    IsActive = true
                };
                objBookingDBEntities.Users.Add(obj);
                message = "Added.";
            }
            else
            {
                //Edit a User
                User obj = objBookingDBEntities.Users.Single(model => model.Id == objUserViewModel.Id);
                obj.UserName = objUserViewModel.UserName;
                if (!string.IsNullOrEmpty(objUserViewModel.PassWord.Trim()))
                {
                    obj.PassWord = objUserViewModel.PassWord;
                }
                obj.FullName = objUserViewModel.FullName;
                obj.Email = objUserViewModel.Email;
                obj.RoleId = objUserViewModel.RoleId;
                obj.IsActive = true;
                message = "Updated.";
            }

            objBookingDBEntities.SaveChanges();
            return Json(new { message = "User Successfully " + message, success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult EditUserDetails(int userid)
        {
            objBookingDBEntities.Configuration.ProxyCreationEnabled = false;
            var result = objBookingDBEntities.Users.Single(model => model.Id == userid);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult DeleteUserDetails(int userid)
        {
            User objUser = objBookingDBEntities.Users.Single(model => model.Id == userid);
            objUser.IsActive = false;
            objBookingDBEntities.SaveChanges();
            return Json(new { message = "Record Successfully Deleted.", success = true }, JsonRequestBehavior.AllowGet);

        }
        //Load All User
        [HttpPost]
        public PartialViewResult GetAllUsers(UserViewModel objUserView)
        {
            IEnumerable<UserDetailsViewModel> listofUserDetailsViewModel =
                (from objUser in objBookingDBEntities.Users
                 where objUser.IsActive == true && (string.IsNullOrEmpty(objUserView.NameSearch.Trim()) ? true : objUser.FullName.StartsWith(objUserView.NameSearch))
                 select new UserDetailsViewModel()
                 {
                     UserName = objUser.UserName,
                     FullName = objUser.FullName,
                     Email = objUser.Email,
                     RoleName = objUser.Role.Name,
                     Id = objUser.Id
                 }).ToList();
            return PartialView("_UserDetailsPartial", listofUserDetailsViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }
        //Verify that the user exists in the system
        [HttpPost]
        public ActionResult Login(LoginViewModel u)
        {
            //this action is for handle post (login)
            if (ModelState.IsValid)// this is check validity
            {
                var User = objBookingDBEntities.Users.Where(x => x.UserName.Equals(u.UserName) && x.PassWord.Equals(u.PassWord)).FirstOrDefault();
                if (User != null)
                {
                    Session["UserName"] = User.UserName.ToString();
                    Session["LogedUserID"] = User.Id.ToString();
                    Session["RoleName"] = User.Role.Name.ToString();
                    Session["LogedFullName"] = User.FullName.ToString();
                    return RedirectToAction("Index", "RoomUsage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName or PassWord.");
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            //this action is for handle post (login)
            Session["LogedUserID"] = null;
            Session["LogedFullName"] = null;
            Session["RoleName"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}