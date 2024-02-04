namespace ScoreManagementClient.Dtos.User.Request
{
    public class UpdateUserRequest : CreateUserRequest
    {
        public string Id { get; set; }
        public bool Active { get; set; }
    }
}
