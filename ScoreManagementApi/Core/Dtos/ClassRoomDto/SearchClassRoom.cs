using ScoreManagementApi.Core.Dtos.ClassRoomDto.Response;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto
{
    public class SearchClassRoom : SearchList<ClassResponse>
    {
        public string? Name { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public bool? Active { get; set; }
        public bool? IsCurrentClass { get; set; }

        public new void ValidateInput()
        {
            base.ValidateInput();

            if(!String.IsNullOrEmpty(Name))
            {
                Name = Name.Trim().ToLower();
            }

            if(IsCurrentClass == null) 
                IsCurrentClass = false;

            if(!String.IsNullOrEmpty(TeacherName))
                TeacherName = TeacherName.Trim().ToLower();

            if(!String.IsNullOrEmpty(SubjectName))
                SubjectName = SubjectName.Trim().ToLower();
        }

    }
}
