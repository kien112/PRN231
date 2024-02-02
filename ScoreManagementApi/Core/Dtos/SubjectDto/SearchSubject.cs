***REMOVED***using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto.Response;

namespace ScoreManagementApi.Core.Dtos.SubjectDto
***REMOVED***
    public class SearchSubject : SearchList<SubjectResponse>
    ***REMOVED***
        public string? Name ***REMOVED*** get; set; ***REMOVED***
        public bool? Active ***REMOVED*** get; set; ***REMOVED***
        public bool? IsCurrentSubject ***REMOVED*** get; set; ***REMOVED***

        public new void ValidateInput()
        ***REMOVED***
            base.ValidateInput();

            if (!String.IsNullOrEmpty(Name))
            ***REMOVED***
                Name = Name.Trim().ToLower();
    ***REMOVED***

            if(IsCurrentSubject == null)
                IsCurrentSubject = false;

***REMOVED***
***REMOVED***
***REMOVED***
