using Microsoft.AspNetCore.Identity;

namespace BloodBridge.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? District { get; set; }
        public string? BloodGroup { get; set; }
        public bool? CanDonate { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime LastDonate { get; set; }

    }
}
