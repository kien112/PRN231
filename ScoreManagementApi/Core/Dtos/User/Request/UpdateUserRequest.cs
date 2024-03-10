using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.User.Request
{
    public class UpdateUserRequest : CreateUserRequest
    {
        public string? Id { get; set; }
        public bool? Active { get; set; }

        public new List<ErrorMessage> ValidateInput(bool IsUpdate)
        {
            var errors = base.ValidateInput(true);

            if (String.IsNullOrEmpty(Id))
                errors.Add(new ErrorMessage
                {
                    Key = "Id",
                    Message = "Id is required!"
                });

            Active ??= false;

            return errors;
        }
    }
}
