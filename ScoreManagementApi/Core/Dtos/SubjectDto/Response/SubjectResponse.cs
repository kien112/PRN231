using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Response;

namespace ScoreManagementApi.Core.Dtos.SubjectDto.Response
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserTiny? Creator { get; set; }
        public List<ComponentScoreResponse>? ComponentScores { get; set; }
    }
}
