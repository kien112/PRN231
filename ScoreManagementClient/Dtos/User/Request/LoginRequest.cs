namespace ScoreManagementClient.Dtos.User.Request
{
    public class LoginRequest
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
