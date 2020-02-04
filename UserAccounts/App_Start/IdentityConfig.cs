using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using UserAccounts;
using UserAccounts.Models;

namespace UserAccounts
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return SendGridAsync(message);
        }

        private async Task SendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.From = new EmailAddress(
                "Joe@contoso.com", "Joe S.");
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;
            myMessage.AddTo(message.Destination);

            var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));
            var azureServiceTokenProvider2 = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://management.azure.com/")
                .ConfigureAwait(false);

            var sendGridApiKey = await keyVaultClient.GetSecretAsync(
                    "https://useraccountskeys.vault.azure.net/secrets/SendGridApiKey/69de5d4a361348cc84000d38469f219a")
                .ConfigureAwait(false);


            var apiKey = sendGridApiKey.Value;

            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(myMessage);
        }
    }
}


public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
        IOwinContext context)
    {
        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
        {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
        };

        manager.PasswordValidator = new PasswordValidator
        {
            RequiredLength = 6,
            RequireNonLetterOrDigit = false,
            RequireDigit = false,
            RequireLowercase = false,
            RequireUppercase = false
        };

        manager.UserLockoutEnabledByDefault = true;
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        manager.EmailService = new EmailService();
        var dataProtectionProvider = options.DataProtectionProvider;
        if (dataProtectionProvider != null)
        {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }

        return manager;
    }
}

public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
{
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
    {
    }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
        return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
    }

    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
        IOwinContext context)
    {
        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
}