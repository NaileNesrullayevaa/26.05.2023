using Delicious.Models;
using Delicious.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Delicious.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            AppUser user=new AppUser()
            {
                Name=register.Name,
                Email=register.Email,
                Surname=register.Surname,
                UserName=register.UserName,
            };
            IdentityResult identityResult=await _userManager.CreateAsync(user,register.Password);
            if (!identityResult.Succeeded)
            {
                foreach(var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
           AppUser user =await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return View(login);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult=await _signInManager.PasswordSignInAsync(user, login.Password,true,false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "email or password incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
