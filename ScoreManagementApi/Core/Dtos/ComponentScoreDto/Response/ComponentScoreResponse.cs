using ScoreManagementApi.Core.Dtos.SubjectDto;

namespace ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response
{
    public class ComponentScoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Percent { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }
        public SubjectTiny? Subject { get; set; } = null;
    }
}
