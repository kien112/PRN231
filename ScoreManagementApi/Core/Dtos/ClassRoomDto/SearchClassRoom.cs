***REMOVED***using ScoreManagementApi.Core.Dtos.ClassRoomDto.Response;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto
***REMOVED***
    public class SearchClassRoom : SearchList<ClassResponse>
    ***REMOVED***
        public string? Name ***REMOVED*** get; set; ***REMOVED***
        public string? TeacherId ***REMOVED*** get; set; ***REMOVED***
        public string? TeacherName ***REMOVED*** get; set; ***REMOVED***
        public int? SubjectId ***REMOVED*** get; set; ***REMOVED***
        public string? SubjectName ***REMOVED*** get; set; ***REMOVED***
        public bool? Active ***REMOVED*** get; set; ***REMOVED***
        public bool? IsCurrentClass ***REMOVED*** get; set; ***REMOVED***

        public new void ValidateInput()
        ***REMOVED***
            base.ValidateInput();

            if(!String.IsNullOrEmpty(Name))
            ***REMOVED***
                Name = Name.Trim().ToLower();
    ***REMOVED***

            if(IsCurrentClass == null) 
                IsCurrentClass = false;

            if(!String.IsNullOrEmpty(TeacherName))
                TeacherName = TeacherName.Trim().ToLower();

            if(!String.IsNullOrEmpty(SubjectName))
                SubjectName = SubjectName.Trim().ToLower();
***REMOVED***

***REMOVED***
***REMOVED***
