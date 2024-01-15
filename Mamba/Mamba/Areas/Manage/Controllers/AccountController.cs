using Mamba.Areas.Manage.ViewModels;
using Mamba.Models;
using Mamba.Utilities.Enums;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }
        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
