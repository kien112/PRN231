using ScoreManagementClient.Dtos.ClassRoomDto.Response;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.SubjectDto;
using ScoreManagementClient.Dtos.User;

namespace ScoreManagementClient.Dtos.ClassRoomDto
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
