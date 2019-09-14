using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult UserDetails(string id)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.SingleOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return View("Index");
                }

                var campaigns = db.CampaignModels.Where(x => x.OwnerId == id).ToList();

                return View(GetUserViewModel(user, campaigns));
            }
        }

        public UserViewModel GetUserViewModel(ApplicationUser user, List<CampaignModel> campaigns)
        {
            var userViewModel = new UserViewModel
            {
                Campaigns = campaigns,
                Id = user.Id,
                UserName = user.UserName,
                Status = ParseStatus(user.LockoutEndDateUtc),
                Role = GetUserRole(user)
            };
            return userViewModel;
        }

        [Authorize]
        public ActionResult UserList()
        {
            ViewBag.Message = "The user list.";

            using (var db = new ApplicationDbContext())
            {
                var viewModel = db.Users.ToList().Select(x => new UserListViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Status = ParseStatus(x.LockoutEndDateUtc),
                    Role = GetUserRole(x)
                });

                return View(viewModel);
            }
        }

        private string GetUserRole(ApplicationUser user)
        {
            return user.IsInRole("Admin") ? "Admin" : "User";
        }

        private string ParseStatus(DateTime? lockoutEnd)
        {
            return lockoutEnd > DateTime.Now ? "Locked" : "Active";
        }
    }
}