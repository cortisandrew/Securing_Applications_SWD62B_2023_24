using Microsoft.AspNetCore.Identity;

namespace Securing_Applications_SWD62B_2023_24.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? SocialSecurityNumber { get; set; }

        public string? Address { get; set; }

        public virtual ICollection<Appraisal>? Appraisals { get; set; }


        
    }
}
