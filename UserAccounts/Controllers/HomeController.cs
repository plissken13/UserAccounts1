using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UserAccounts.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;

namespace UserAccounts.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            // Instantiate a new KeyVaultClient object, with an access token to Key Vault
            var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));

            // Optional: Request an access token to other Azure services
            var azureServiceTokenProvider2 = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://management.azure.com/").ConfigureAwait(false);

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
        [Authorize]
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

        [Authorize(Roles = "Admin")]
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