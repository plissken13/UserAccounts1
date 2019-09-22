using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserAccounts.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public bool IsInRole(string roleName)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            return manager.IsInRole(Id, roleName);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CampaignModel> CampaignModels { get; set; }

        public DbSet<PostModel> PostModels { get; set; }

        public DbSet<CommentsModel> CommentsModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostModel>()
                .HasRequired(s => s.Campaign)
                .WithMany(g => g.Posts)
                .HasForeignKey(s => s.CampaignId);
            modelBuilder.Entity<CommentsModel>()
                .HasRequired(s => s.Campaign)
                .WithMany(g => g.Comments)
                .HasForeignKey(s => s.CampaignId);
            base.OnModelCreating(modelBuilder);
        }
    }
}