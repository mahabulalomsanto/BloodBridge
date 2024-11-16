using BloodBridge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBridge.Controllers
{
    public class DonorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public DonorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var donors = _userManager.Users.ToList();
            donors = donors.Where(d => d.CanDonate == true).ToList();
            return View(donors);
        }


        public async Task<IActionResult> DonationInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        public async Task<IActionResult> EditDonorInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditDonorInfo(ApplicationUser obj)
        {

            var user = await _userManager.GetUserAsync(User);
            user.CanDonate = obj.CanDonate;
            user.LastDonate = obj.LastDonate;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("DonationInfo");
        }


        [HttpGet]
        public async Task<IActionResult> Search(string District, string BG)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(District) || !string.IsNullOrEmpty(BG))
            {
                users = users.Where(u =>
                    (u.CanDonate == true) &&
                    (string.IsNullOrEmpty(District) || u.District.Contains(District)) &&
                    (string.IsNullOrEmpty(BG) || u.BloodGroup.Contains(BG)));
            }
            var filteredUsers = await users.ToListAsync();
            return View("Index", filteredUsers);
        }

        public IActionResult Contact(string Id)
        {
            var user = _userManager.FindByIdAsync(Id);

            return View(user);
        }




    }

}
