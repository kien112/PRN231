namespace ScoreManagementApi.Core.Dtos.User.Request
{
    public class UpdateUserRequest : CreateUserRequest
    {
        public string Id { get; set; }
        public bool Active { get; set; }
    }
}
