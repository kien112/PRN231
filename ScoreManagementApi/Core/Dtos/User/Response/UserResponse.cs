using Microsoft.AspNetCore.Identity;
using ScoreManagementApi.Core.OtherObjects;

namespace ScoreManagementApi.Core.Dtos.User.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public bool Active { get; set; }
        public List<string> Roles { get; set; }

    }
}
