using BloodBridge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloodBridge.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> GetPhoto(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return File(user.Photo, "image/jpeg"); 
        }





        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

           public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUser obj, IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);

            user.Name = obj.Name;
            user.Email = obj.Email;
            user.Phone = obj.Phone;
            user.District = obj.District;
            user.BloodGroup = obj.BloodGroup;
            user.CanDonate = obj.CanDonate;
            user.LastDonate = obj.LastDonate;

            if(photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    user.Photo = memoryStream.ToArray(); // Convert the uploaded image to a byte array
                }
            }


            await _userManager.UpdateAsync(user);

            return RedirectToAction("Userinfo", "User");
        }
    }
}
