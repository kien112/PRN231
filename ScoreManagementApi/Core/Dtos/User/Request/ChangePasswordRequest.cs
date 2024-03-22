using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.User.Request
{
    public class ChangePasswordRequest
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

        public List<ErrorMessage> ValidateInput()
        {
            var errors = new List<ErrorMessage>();

            if (String.IsNullOrEmpty(OldPassword))
                errors.Add(new ErrorMessage
                {
                    Key = "OldPassword",
                    Message = "Old Password is required!"
                });
           
            if (String.IsNullOrEmpty(NewPassword))
                errors.Add(new ErrorMessage
                {
                    Key = "NewPassword",
                    Message = "New Password is required!"
                });
            else if(NewPassword.Length < 6 || NewPassword.Length > 250)
                errors.Add(new ErrorMessage
                {
                    Key = "NewPassword",
                    Message = "Length of New Password must be in range 6 to 250!"
                });

            if (String.IsNullOrEmpty(ConfirmPassword))
                errors.Add(new ErrorMessage
                {
                    Key = "ConfirmPassword",
                    Message = "Confirm Password is required!"
                });

            if(!String.IsNullOrEmpty(ConfirmPassword) && !String.IsNullOrEmpty(NewPassword) 
                && !ConfirmPassword.Equals(NewPassword))
                errors.Add(new ErrorMessage
                {
                    Key = "Message",
                    Message = "New Password must be equal to Confirm Password!"
                });

            return errors;
        }
    }
}
