using Microsoft.AspNetCore.Identity;

namespace ScoreManagementApi.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public bool Active { get; set; }
        public bool Gender { get; set; }

    }
}
