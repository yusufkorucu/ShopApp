using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Extentios;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender,ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;

        }
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {


                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //generate token
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = code
                    });
                    //send mail
                    await _emailSender.SendEmailAsync(model.Email, "Confirmed Account", $"Lütfen Mail Hesabınızı onaylama için linke <a href='http://localhost:65091{callbackUrl}'> tıklayın</a>");
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Eposta Adresinizi Hesabınızı doğrulamanız için onaylama linki gönderildi!",
                        Css = "warning"
                    });
                    return RedirectToAction("login", "account");

                }
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu tekrar deneyiniz.Şifrenizde en az bir büyük bir küçük harf birde rakam kullanmalısınız");
            return View(model);
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Böyle bir Kullanıcı Yok");
                return View();

            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen Hesabınızı email ile onaylayınız!!");
                return View(model);

            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/home/product/");

            }
            ModelState.AddModelError("", "Email veya Parola Yanlış");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap Onayı İçin Bilgiler Yanlış!!",
                    Css = "danger"
                });
                return Redirect("~/");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //Create Card Object
                    _cartService.InitializeCart(user.Id);
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız Başarıyla Onaylanmıştır",
                        Css = "success"
                    });
                    return RedirectToAction("Login");

                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız Onaylanamadı",
                Css = "danger"
            });

            return RedirectToAction("~/");
        }
        public IActionResult FargotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FargotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });
                return View();

            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "E posta adresi ile alakalı bir kullanıcı bulunmadı",
                    Css = "danger"
                });
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {

                token = code,
            });
            //send mail
            await _emailSender.SendEmailAsync(Email, "Reset Passowrd", $"Paralola değişikliği için  <a href='http://localhost:65091{callbackUrl}' >tıklayın</a>");
            TempData.Put("message", new ResultMessage()
            {
                Title = "Forgot Password",
                Message = "Parola Yenilemek için Mail Hesabınza bağlantı gönderildi",
                Css = "warning"
            });

            return RedirectToAction("login", "account");


        }
        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {

                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel
            {
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");

            }
            return View();
        }
        public IActionResult Acccessdenied()
        {
            return View();
        }
    }
}
