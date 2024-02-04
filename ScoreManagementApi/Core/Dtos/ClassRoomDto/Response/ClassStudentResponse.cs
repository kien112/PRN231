
using ScoreManagementApi.Dtos.User;

namespace ScoreManagementApi.Dtos.ClassRoomDto.Response
{
    public class ClassStudentResponse
    {
        public int Id { get; set; }
        public UserTiny Student { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
