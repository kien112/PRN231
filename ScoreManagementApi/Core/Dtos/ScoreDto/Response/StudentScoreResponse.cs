using ScoreManagementApi.Core.Dtos.ComponentScoreDto;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Response
{
    public class StudentScoreResponse
    {
        public ComponentScoreTiny ComponentScore { get; set; }
        public float? Mark { get; set; }
    }
}
