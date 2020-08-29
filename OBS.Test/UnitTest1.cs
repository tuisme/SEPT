using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineBookingSystem;
using OnlineBookingSystem.Controllers;
using OnlineBookingSystem.ViewModel;
using System.Web.Mvc;
using System.Threading.Tasks;
using OnlineBookingSystem.Models;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Reflection;
using Moq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Configuration;

namespace OBS.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Item1_Login()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();
            context.Setup(m => m.HttpContext.Session).Returns(session);

            AccountController _acctController = new AccountController();
            _acctController.ControllerContext = context.Object;

            LoginViewModel md = new LoginViewModel();
            md.UserName = "admin";
            md.PassWord = "123";

            var result = _acctController.Login(md) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("RoomUsage", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void Item2_SendBookingRequest()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();
            context.Setup(m => m.HttpContext.Session).Returns(session);

            BookingController bc = new BookingController();
            bc.ControllerContext = context.Object;

            bc.Session["LogedUserID"] = 3;

            BookingViewModel bvmd = new BookingViewModel()
            {
                RoomId = 16,
                BookingHourID = 1,
                BookingDate = DateTime.Today.AddDays(4),
                NumberOfMembers = "5"
            };

            var result = bc.Index(bvmd) as JsonResult;

            string test = Convert.ToString(result.Data);

            Assert.AreEqual("{ message = Booking Successfully Created., success = True }", Convert.ToString(result.Data));
        }

        [TestMethod]
        public void Item3_ListOfRooms()
        {
            BookingController bc = new BookingController();

            var result = bc.Index() as ViewResult;

            var test = result.ViewData.Model as BookingViewModel;

            //the count value may be diferent.  At the time for testing this function, there are total 6 Rooms
            Assert.AreEqual(6, test.ListOfRooms.Count());
        }

        [TestMethod]
        public void Item4_LoadRoomAndTimeSlotByDate()
        {

            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();
            context.Setup(m => m.HttpContext.Session).Returns(session);

            BookingController bc = new BookingController();
            bc.ControllerContext = context.Object;

            bc.Session["UserName"] = "admin";

            BookingViewModel bvmd = new BookingViewModel() { BookingDate = DateTime.Today };

            var result = bc.GetAllRooms(bvmd) as PartialViewResult;

            IEnumerable<BookingViewModel> mds = result.Model as IEnumerable<BookingViewModel>;

            //the count value may be diferent.  At the time for testing this function, there are total 6 Rooms
            Assert.AreEqual(6, mds.Count());
        }

        [TestMethod]
        public void Item5_MyBookings()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();

            context.Setup(m => m.HttpContext.Session).Returns(session);
            RoomUsageController rc = new RoomUsageController();
            rc.ControllerContext = context.Object;
            rc.Session["UserName"] = "admin";

            var result = rc.GetAllBookingHistory() as PartialViewResult;

            IEnumerable<RoomUsageDetailsViewModel> mds = result.Model as IEnumerable<RoomUsageDetailsViewModel>;

            //the count value may be diferent. At the time for testing this function, there are total 9 MyBookings
            Assert.AreEqual(9, mds.Count());
        }

        [TestMethod]
        public void Item6_DeleteBooking()
        {
            RoomUsageController rc = new RoomUsageController();

            //valid ID when delete: from 37 o 42
            var result = rc.DeleteRoomUsageDetails(20) as JsonResult;

            string a = Convert.ToString(result.Data);

            Assert.AreEqual("{ message = Record Successfully Deleted., success = True }", Convert.ToString(result.Data));
        }

        [TestMethod]
        public void Item6_ViewAllBookingswithAdmin()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();

            context.Setup(m => m.HttpContext.Session).Returns(session);
            RoomUsageController rc = new RoomUsageController();
            rc.ControllerContext = context.Object;
            rc.Session["UserName"] = "admin";   // this is the login account to obs.

            var result = rc.GetAllBookingHistory() as PartialViewResult;

            IEnumerable<RoomUsageDetailsViewModel> mds = result.Model as IEnumerable<RoomUsageDetailsViewModel>;

            //the count value may be diferent. At the time for testing this function, there are total 29 MyBookings

            /* Use below query to know the exact booking number in database:
              
                select	count(*)
                from	RoomUsages
                where	IsActive = 1

             */
            Assert.AreEqual(29, mds.Count());
        }

        [TestMethod]
        public void Item7_ForceCancelButton()
        {
            //Inject the configurations value - no need to change values in this block
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();
            config.AppSettings.Settings.Add("FromEmailAddress", "obs.helpline@gmail.com");
            config.AppSettings.Settings.Add("FromEmailDisplayName", "Booking Admin");
            config.AppSettings.Settings.Add("FromEmailPassWord", "obsg11s2");
            config.AppSettings.Settings.Add("SMTPHost", "smtp.gmail.com");
            config.AppSettings.Settings.Add("SMTPPort", "587");
            config.AppSettings.Settings.Add("EnableSSL", "true");

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            //End Injection

            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();

            var serverMock = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);

            //NOTE: you need to change the parameter in below Returns function to the path on your machine.
            //The path should be "Your Project Foder Path\Views\Shared\_TemplateSendEmail.cshtml"
            serverMock.Setup(x => x.MapPath("~/Views/Shared/_TemplateSendEmail.cshtml")).
                Returns(@"D:\Webapp\OBS\Views\Shared\_TemplateSendEmail.cshtml");

            context.Setup(m => m.HttpContext.Session).Returns(session);
            context.Setup(x => x.HttpContext.Server).Returns(serverMock.Object);

            RoomUsageController rc = new RoomUsageController();
            rc.ControllerContext = context.Object;
            rc.Session["UserName"] = "admin";   // this is the login account to obs.

            var bookingHis = rc.GetAllBookingHistory() as PartialViewResult;
            IEnumerable<RoomUsageDetailsViewModel> mds = bookingHis.Model as IEnumerable<RoomUsageDetailsViewModel>;
            int beforeCancel = mds.Count(); // get number of booking before clicking force cancel

            rc.DeleteRoomUsageDetails(24); // Force Cancel one booking, value 24 is the parameter of the booking ID.

            /* Use below query to know the exact booking id in database:
              
                select	ID,*
                from	RoomUsages
                where	IsActive = 1

             */

            var currentBooking = rc.GetAllBookingHistory() as PartialViewResult;
            IEnumerable<RoomUsageDetailsViewModel> currB = currentBooking.Model as IEnumerable<RoomUsageDetailsViewModel>;
            int afterCancel = currB.Count(); // get number of booking after clicking force cancel


            Assert.AreEqual(beforeCancel - 1, afterCancel); // compare number of bookings before and after cancel
        }


        [TestMethod]
        public void Item8_ListRoomsByStaff()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();
            context.Setup(m => m.HttpContext.Session).Returns(session);

            BookingController bc = new BookingController();
            bc.ControllerContext = context.Object;

            bc.Session["UserName"] = "staff"; // this is the login account to obs.

            BookingViewModel bvmd = new BookingViewModel() { BookingDate = DateTime.Today };

            var result = bc.GetAllRooms(bvmd) as PartialViewResult;

            IEnumerable<BookingViewModel> mds = result.Model as IEnumerable<BookingViewModel>;

            //the count value may be diferent.  At the time for testing this function, there are total 6 Rooms

            /* Use below query to know the exact booking number in database:
              
                select	count(*)
                from	Rooms
                where	IsActive = 1

             */

            Assert.AreEqual(6, mds.Count());
        }

        [TestMethod]
        public void Item9_ListRoomsByStudent()
        {
            var context = new Mock<ControllerContext>();
            var session = new MockHttpSession();
            context.Setup(m => m.HttpContext.Session).Returns(session);

            BookingController bc = new BookingController();
            bc.ControllerContext = context.Object;

            bc.Session["UserName"] = "student"; // this is the login account to obs.

            BookingViewModel bvmd = new BookingViewModel() { BookingDate = DateTime.Today };

            var result = bc.GetAllRooms(bvmd) as PartialViewResult;

            IEnumerable<BookingViewModel> mds = result.Model as IEnumerable<BookingViewModel>;

            //the count value may be diferent.  At the time for testing this function, there are total 6 Rooms

            /* Use below query to know the exact booking number in database:
              
                select	count(*)
                from	Rooms
                where	IsActive = 1

             */

            Assert.AreNotEqual(6, mds.Count());
        }
        [TestMethod]
        public void Item10_CreateAccount()
        {
            AccountController rc = new AccountController();
            UserViewModel bvmd = new UserViewModel()
            {
                NameSearch = "",
                UserName = "Student101",
                PassWord = "123",
                FullName = "Student101 Test",
                Email = "student101.OBS2@gmail.com",
                IsActive = true,
                RoleId = 1,
                Id = 0
            };
            var listUsers = rc.GetAllUsers(bvmd) as PartialViewResult;
            IEnumerable<UserDetailsViewModel> mds = listUsers.Model as IEnumerable<UserDetailsViewModel>;
            int beforeCreate = mds.Count(); // get total number of users before create new

            rc.Index(bvmd); //Crete new user

            var currentBooking = rc.GetAllUsers(bvmd) as PartialViewResult;
            IEnumerable<UserDetailsViewModel> currB = currentBooking.Model as IEnumerable<UserDetailsViewModel>;
            int afterCreate = currB.Count(); // get total number of users after create 1 new user

            Assert.AreEqual(beforeCreate + 1, afterCreate); //compare number of users before and after create
        }
    }

}

