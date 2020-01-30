using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using UserAccounts.Models;

[assembly: OwinStartupAttribute(typeof(UserAccounts.Startup))]

namespace UserAccounts
{
    public partial class Startup
    {
        public async void Configuration(IAppBuilder app)
        {
            await ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }

            var user = new ApplicationUser();

            user.Email = "adm2309@mailinator.com";
            user.EmailConfirmed = true;
            user.UserName = user.Email;

            string userPWD = "123qwe";

            var chkUser = UserManager.Create(user, userPWD);

            //Add default User to Role Admin   
            if (chkUser.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}