using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
{
    public class ClassStudent
    {
        public int Id { get; set; }
        [ForeignKey("ClassRoom")]
        public int ClassRoomId { get; set; }
        public ClassRoom ClassRoom { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public User Student { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
