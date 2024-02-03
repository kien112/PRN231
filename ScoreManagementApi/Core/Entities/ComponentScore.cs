using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
{
    public class ComponentScore
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        public float Percent { get; set; }
        public bool Active { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
