using ScoreManagementClient.Dtos.ComponentScoreDto;
using ScoreManagementClient.Dtos.User;

namespace ScoreManagementClient.Dtos.ScoreDto.Response
{
    public class ScoreResponse
    {
        public List<StudentScore> StudentScores { get; set; }
        public List<ComponentScoreTiny> ComponentScore { get; set; }
        public int? TotalElements { get; set; }
        public string? OrderBy { get; set; }
        public string? SortBy { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }

    public class StudentScore
    {
        public UserTiny Student { get; set; }
        public List<ComponentScoreIdAndMark> ComponentScoreIdAndMarks { get; set; }
    }

    public class ComponentScoreIdAndMark
    {
        public int Id { get; set; }
        public float? Mark { get; set; }
    }
}
