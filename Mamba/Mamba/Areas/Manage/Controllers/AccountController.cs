using Mamba.Areas.Manage.ViewModels;
using Mamba.Models;
using Mamba.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Mamba.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            if (!registerVM.Name.Check())
            {
                ModelState.AddModelError("Name", "Wrong format");
                return View();
            }
            if (!registerVM.Surname.Check())
            {
                ModelState.AddModelError("Surname", "Wrong format");
                return View();
            }

            string email = registerVM.Email;
            Regex regex = new Regex(@"^(([0-9a-z]|[a-z0-9(\.)?a-z]|[a-z0-9])){1,}(\@)[a-z((\-)?)]{1,}(\.)([a-z]{1,}(\.))?([a-z]{2,3})$");
            if (!regex.IsMatch(email))
            {
                ModelState.AddModelError("Surname", "Wrong format");
                return View();
            }

            AppUser user = new AppUser
            {
                Name = registerVM.Name.Trim().Capitalize(),
                Surname = registerVM.Surname.Trim().Capitalize(),
                UserName = registerVM.Username.Trim(),
                Email = registerVM.Email.Trim(),
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                    return View();  
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index","Dashboard");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnurl)
        {
            if(!ModelState.IsValid)return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError(String.Empty, "Username, Email or Password is incorrect");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemembered, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "U r blocked");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username, Email or Password is incorrect");
                return View();
            }
            if(returnurl == null)
            {
                return RedirectToAction("Index", "Dashboard");

            }
            return Redirect(returnurl);
        }
    }
}
