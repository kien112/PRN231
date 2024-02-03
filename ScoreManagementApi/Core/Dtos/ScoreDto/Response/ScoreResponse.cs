using ScoreManagementApi.Core.Dtos.ComponentScoreDto;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Response
{
    public class ScoreResponse
    {
        public int Id { get; set; }
        public UserTiny Student { get; set; }
        public ComponentScoreTiny ComponentScore { get; set; }
        public float? Mark { get; set; }
    }
}
