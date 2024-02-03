
namespace ScoreManagementApi.Core.Dtos.User.Request
{
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }

    }
}
