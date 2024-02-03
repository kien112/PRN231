using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.SubjectDto;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Response
{
    public class ClassResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public UserTiny? Teacher { get; set; }
        public UserTiny? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public SubjectTiny Subject { get; set; }
        public List<ClassStudentResponse> ClassStudents { get; set; }
    }
}
