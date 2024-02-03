using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
{
    public class Subject
    {

        public int Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; } = null!;
        
        [MaxLength(250)]
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("Creator")]
        public string? CreatorId { get; set; }
        public User? Creator { get; set; }

        public virtual List<ClassRoom>? ClassRooms { get; set; }
        public virtual List<ComponentScore>? ComponentScores { get; set; }
    }
}
