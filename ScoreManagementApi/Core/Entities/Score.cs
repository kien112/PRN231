using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Entities
{
    public class Score
    {
        public int Id { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public User Student { get; set; }

        [ForeignKey("ComponentScore")]
        public int ComponentScoreId { get; set; }
        public ComponentScore ComponentScore { get; set; }

        public float? Mark { get; set; }
    }
}
