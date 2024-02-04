using ScoreManagementClient.Dtos.ComponentScoreDto;
using ScoreManagementClient.Dtos.User;

namespace ScoreManagementClient.Dtos.ScoreDto.Response
{
    public class ScoreResponse
    {
        public int Id { get; set; }
        public UserTiny Student { get; set; }
        public ComponentScoreTiny ComponentScore { get; set; }
        public float? Mark { get; set; }
    }
}
