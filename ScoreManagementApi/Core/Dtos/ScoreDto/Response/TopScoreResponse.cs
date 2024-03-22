using ScoreManagementApi.Core.Dtos.ClassRoomDto;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Response
{
    public class TopScoreResponse
    {
        public List<TopScore> TopScores { get; set; }
        public TopScore? OwnerRank { get; set; }
    }

    public class TopScore
    {
        public UserTiny Student { get; set; }
        public SubjectTiny Subject { get; set; }
        public ClassRoomTiny ClassRoom { get; set; }
        public float Score { get; set; }
        public int Rank { get; set; }
    }
}
