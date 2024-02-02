***REMOVED***using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.OtherObjects;

namespace ScoreManagementApi.Core.Dtos.User
***REMOVED***
    public class SearchUsers : SearchList<UserResponse>
    ***REMOVED***
        public string? KeyWord ***REMOVED*** get; set; ***REMOVED***
        public string? Role ***REMOVED*** get; set; ***REMOVED***
        public string? Gender ***REMOVED*** get; set; ***REMOVED***

        public new void ValidateInput()
        ***REMOVED***
            base.ValidateInput();
            
            if (KeyWord != null)
                KeyWord = KeyWord.ToLower().Trim();

            if (Role != null && !Role.ToUpper().Equals(StaticUserRoles.ADMIN)
                && !Role.ToUpper().Equals(StaticUserRoles.STUDENT) && !Role.ToUpper().Equals(StaticUserRoles.TEACHER))
                Role = null;

            if(Gender != null && !Gender.ToUpper().Equals(StaticString.FEMALE) 
                && !Gender.ToUpper().Equals(StaticString.MALE))
                Gender = null;
***REMOVED***
***REMOVED***
***REMOVED***
