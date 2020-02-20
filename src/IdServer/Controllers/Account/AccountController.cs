using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdServer.Data;
using IdServer.Models;
using IdServer.Controllers.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using IdServer.Services;

namespace IdentityServer4.Controllers.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly INameGenerator _nameGenerator;
        private readonly IColorGenerator _colorGenerator;
        private readonly IPasswordGenerator _passwordGenerator;

        public AccountController(
            ApplicationDbContext db,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            INameGenerator nameGenerator,
            IColorGenerator colorGenerator,
            IPasswordGenerator passwordGenerator)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _nameGenerator = nameGenerator;
            _colorGenerator = colorGenerator;
            _passwordGenerator = passwordGenerator;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Account/LoggedIn")
        {
            LoginViewModel user = new LoginViewModel();
            user.ReturnUrl = returnUrl;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            string password = model.Password.ToLower();
            string userName = _nameGenerator.FilterForDb(model.UserName);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(userName, user.Id, user.UserName, clientId: context?.ClientId));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        return Redirect(model.ReturnUrl);
                    }

                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("/Account/Login");
                    }
                    else
                    {
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(userName, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult LoggedIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = "/Account/LoggedIn")
        {
            HttpContext.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline' 'self'");
            RegisterViewModel user = new RegisterViewModel();
            user.ReturnUrl = returnUrl;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User()
                    {
                        UserName = model.UserName,
                        Color1 = model.Color1,
                        Color2 = model.Color2
                    };
                    var result = await _userManager.CreateAsync(user, model.Password.ToLower());
                    LoginViewModel loginModel = new LoginViewModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        ReturnUrl = model.ReturnUrl
                    };
                    return await Login(loginModel);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            return View(model);
        }

        public async Task<string> GetNewName(int timeout = 0)
        {
            if (timeout >= _nameGenerator.NumberOfPossibleNames)
            {
                throw new Exception("Error trying to generate Username. Try again later");
            }
            string name = _nameGenerator.GenerateName();
            var exists = await _db.Users.FirstOrDefaultAsync(e => e.UserName == name);
            if (exists != null)
            {
                return await GetNewName(timeout + 1);
            }
            return name;
        }

        public async Task<PartialViewResult> UserName()
        {
            try
            {
                ViewData["UserName"] = await GetNewName();
                return PartialView("_UserName");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public PartialViewResult Password()
        {
            ViewData["Password"] = _passwordGenerator.GeneratePassword();
            return PartialView("_Password");
        }

        public PartialViewResult Colors()
        {
            ViewData["Colors"] = _colorGenerator.GenerateColors();
            return PartialView("_Colors");
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                return await Logout(vm);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}