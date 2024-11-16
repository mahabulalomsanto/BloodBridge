using BloodBridge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBridge.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager ;
        private readonly RoleManager<IdentityRole> _roleManager ;


        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddUser()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(ApplicationUser model, string password, string role)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.UserName,  // Email will be used as the UserName
                Name = model.Name,
                BloodGroup = model.BloodGroup,
                District = model.District
            };


            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,role);
                return RedirectToAction("UserList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.ToList();

            var userRoles = new Dictionary<ApplicationUser, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(user, roles.ToList());
            }

            ViewBag.UserRoles = userRoles; 

            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleList");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Role already exists.");
                }

            }

            return View();

        }


        [Authorize(Roles = "Admin")]
        public IActionResult Rolelist()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }




    }
}
