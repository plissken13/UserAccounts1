using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class RolesController : Controller
    {
        protected readonly UserManager<ApplicationUser> UserManager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<bool> SetAdminPermissions(List<string> arr)
        {
            foreach (string id in arr)
            {
                await UserManager.AddToRoleAsync(id, "Admin");
            }

            return true;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<bool> SetUserPermissions(List<string> arr)
        {
            foreach (string id in arr)
            {
                await UserManager.AddToRoleAsync(id, "User");
                await UserManager.RemoveFromRoleAsync(id, "Admin");
            }

            return true;
        }
    }
}