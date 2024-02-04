
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Response
{
    public class ClassStudentResponse
    {
        public int Id { get; set; }
        public UserTiny Student { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
