using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Comics.Domain;
using Comics.Services.Abstract;
using Comics.DAL;
using Comics.Web.ViewModel;
using Microsoft.AspNetCore.Http;
using Comics.Services;

namespace Comics.Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ILogger<AccountController> _logger;
        private IUserRepository _userRep;
        private ICollectionRepository _collectionRepository;
        private IEmail _email;
        private ImageManagment _imageManagment;
        private ComicsDbContext _db;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IUserRepository userRep, ICollectionRepository collectionRepository, IEmail email, ImageManagment imageManagment, ComicsDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userRep = userRep;
            _collectionRepository = collectionRepository;
            _email = email;
            _imageManagment = imageManagment;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = model.Email;
                var user = await _userManager.FindByEmailAsync(userName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Такого пользователя не существует");
                    return View(model);
                }
                else
                {
                    userName = user.UserName;
                }
                if (model.Remember)
                {
                    var result = await _signInManager.PasswordSignInAsync(userName, model.Password, isPersistent: true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Collection");
                    }
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(userName, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Collection");
                    }
                }


            }
            else
            {
                ModelState.AddModelError("", "Неверный пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Name,
                    LockoutEnabled = false,
                    Registration = DateTime.Now.ToUniversalTime(),
                };

                var isEmailExist = await _userManager.FindByEmailAsync(model.Email);
                IdentityResult result;

                if (isEmailExist == null)
                    result = await _userManager.CreateAsync(user, model.Password);
                else
                {
                    ModelState.AddModelError(string.Empty, "Email address is already occupied");
                    return View(model);
                }
                    


                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user"); //was guest

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    var url = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await _email.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите аккаунт, перейдя по ссылке: <a href='{url}'>link</a>");

                    return RedirectToAction("Message");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
            }
            return View(model);
        }

        public IActionResult Message()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {

            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExtrernalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult>
        ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExtrernalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }


            // Получаем информации о пользователе зашедшего через внешнего провайдера
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }


            //Если пользователь уже зарегистрирован в приложении, то просто заходим
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            else
            {

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {

                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        string userName = info.Principal.FindFirstValue(ClaimTypes.Email);
                        string result = userName.Substring(0, userName.LastIndexOf('@'));
                        user = new User
                        {
                            UserName = result,
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Registration = DateTime.UtcNow,
                            EmailConfirmed = true
                        };

                        await _userManager.CreateAsync(user, "temporary");
                        await _signInManager.PasswordSignInAsync(user, "temporary", isPersistent: false, lockoutOnFailure: false);

                    }
                    var res = await _signInManager.PasswordSignInAsync(user, "temporary", isPersistent: false, lockoutOnFailure: false);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("CreatePassword");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, true);
                        return RedirectToAction("Index", "Collection");
                    }

                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";


                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {

            User user = _userRep.GetUserInfo(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ProfileViewModel obj = new ProfileViewModel
            {
                user = user,
                isMe = (currentUser != null && currentUser.Id == id) ? true : false
            };
            return View(obj);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return View("Profile", model.Id);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                _logger.LogError("Wrong userdId or code. Controller:Account. Action:ConfirmEmail");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Doesn't exist user. Controller:Account. Action:ConfirmEmail");
                return RedirectPermanent("~/Error/Index?statusCode=404");

            }
            await _userManager.RemoveFromRoleAsync(user, "guest");
            await _userManager.AddToRoleAsync(user, "user");
            await _signInManager.SignInAsync(user, false);

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Collection");
            else
                return RedirectPermanent("~/Error/Index?statusCode=404");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {

            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var colls = _collectionRepository.GetUserCollections(user);
                _db.Collections.RemoveRange(_db.Collections.Where(l => l.User.Id == id));
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult CreatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePassword(CreatePasswordViewModel model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, "temporary", model.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Collection");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ChangeRole(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            var user_roles = await _userManager.GetRolesAsync(user);
            if (user_roles.Contains("admin"))
                return RedirectToAction("Index", "Users");
            
                

            if (!user_roles.Contains("moderator")) {
                await _userManager.AddToRoleAsync(user, "moderator");
                await _userManager.RemoveFromRoleAsync(user, "user");
            } else
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _userManager.RemoveFromRoleAsync(user, "moderator");
            }

            return RedirectToAction("Index", "Users");
        }

        /*
        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
        */
    }


}