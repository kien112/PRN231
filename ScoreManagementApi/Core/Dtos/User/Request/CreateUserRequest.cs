
using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.User.Request
{
    public class CreateUserRequest
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? Gender { get; set; }

        public List<ErrorMessage> ValidateInput(bool IsUpdate)
        {
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(FullName))
                errors.Add(new ErrorMessage
                {
                    Key = "FullName",
                    Message = "FullName is required!"
                });
            else if (FullName.Length > 250)
                errors.Add(new ErrorMessage
                {
                    Key = "FullName",
                    Message = "Length of FullName must be <= 250"
                });

            if(String.IsNullOrEmpty(UserName))
                errors.Add(new ErrorMessage
                {
                    Key = "UserName",
                    Message = "UserName is required!"
                });
            else if (UserName.Length > 250)
                errors.Add(new ErrorMessage
                {
                    Key = "UserName",
                    Message = "Length of UserName must be <= 250"
                });

            if (String.IsNullOrEmpty(Email))
                errors.Add(new ErrorMessage
                {
                    Key = "Email",
                    Message = "Email is required!"
                });
            else if(Email.Length > 250)
                errors.Add(new ErrorMessage
                {
                    Key = "Email",
                    Message = "Length of Email must be <= 250"
                });

            if(String.IsNullOrEmpty(Password) && !IsUpdate)
                errors.Add(new ErrorMessage
                {
                    Key = "Password",
                    Message = "Password is required!"
                });
            else if(Password != null && (Password.Length < 6 || Password.Length > 250))
                errors.Add(new ErrorMessage
                {
                    Key = "Password",
                    Message = "Length of Password must be >= 6 and <= 250"
                });

            if (Gender == null)
                errors.Add(new ErrorMessage
                {
                    Key = "Gender",
                    Message = "Gender is required!"
                });

            return errors;
        }
    }
}
