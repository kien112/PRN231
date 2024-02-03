using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.OtherObjects;

namespace ScoreManagementApi.Core.Dtos.User
{
    public class SearchUsers : SearchList<UserResponse>
    {
        public string? KeyWord { get; set; }
        public string? Role { get; set; }
        public string? Gender { get; set; }

        public new void ValidateInput()
        {
            base.ValidateInput();
            
            if (KeyWord != null)
                KeyWord = KeyWord.ToLower().Trim();

            if (Role != null && !Role.ToUpper().Equals(StaticUserRoles.ADMIN)
                && !Role.ToUpper().Equals(StaticUserRoles.STUDENT) && !Role.ToUpper().Equals(StaticUserRoles.TEACHER))
                Role = null;

            if(Gender != null && !Gender.ToUpper().Equals(StaticString.FEMALE) 
                && !Gender.ToUpper().Equals(StaticString.MALE))
                Gender = null;
        }
    }
}
