using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
{
    public class ClassRoom
    {
        public int Id { get; set; }
        
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }
        
        public bool Active { get; set; }

        [ForeignKey("Teacher")]
        public string? TeacherId { get; set; }
        public User? Teacher { get; set; }

        [ForeignKey("Creator")]
        public string? CreatorId { get; set; }
        public User? Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual List<ClassStudent> ClassStudents { get; set; }
    }
}
