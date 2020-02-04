using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;
using UserAccounts.Models;

namespace UserAccounts
{
    public partial class Startup
    {
        public async Task ConfigureAuth(IAppBuilder app)
        {
            var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));
            var azureServiceTokenProvider2 = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://management.azure.com/")
                .ConfigureAwait(false);


            var demoSecret = ConfigurationManager.AppSettings["demoSecret"];

            var googleClientId = await keyVaultClient.GetSecretAsync(
                    "https://useraccountskeys.vault.azure.net/secrets/GoogleClientID/0987555de5794d6ab165db0d7cb0c601")
                .ConfigureAwait(false);

            var googleClientSecret = await keyVaultClient.GetSecretAsync(
                    "https://useraccountskeys.vault.azure.net/secrets/GoogleClientSecret/f8234e18205043c2a1d8804f42fc34c6")
                .ConfigureAwait(false);

            var facebookAppId = await keyVaultClient.GetSecretAsync(
                    "https://useraccountskeys.vault.azure.net/secrets/FacebookAppID/10d67e52aec04f8a90730e4b09187170")
                .ConfigureAwait(false);

            var facebookAppSecret = await keyVaultClient.GetSecretAsync(
                    "https://useraccountskeys.vault.azure.net/secrets/FacebookAppSecret/386eade3a4ae444d8916baae7ffaec9f")
                .ConfigureAwait(false);

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));


            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = googleClientId.Value,
                ClientSecret = googleClientSecret.Value
            });

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
            {
                AppId = facebookAppId.Value,
                AppSecret = facebookAppSecret.Value
            });
        }
    }
}