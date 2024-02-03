using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Dtos.ClassRoomDto.Response
{
    public class ClassStudentResponse
    {
        public int Id { get; set; }
        public UserTiny Student { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
