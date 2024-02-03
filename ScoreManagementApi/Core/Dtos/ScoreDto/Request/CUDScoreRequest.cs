using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
{
    public class CUDScoreRequest
    {
        public float? Mark { get; set; }
        public string? StudentId { get; set; }
        public int? ComponentScoreId { get; set; }

        public ErrorMessage? ValidateInput()
        {
            if(StudentId == null ||  ComponentScoreId == null)
            {
                return new ErrorMessage
                {
                    Key = "StudentId and ComponentScoreId",
                    Message = "StudentId and ComponentScoreId is required!"
                };
            }
            else if(Mark != null && (Mark < 0 || Mark > 10))
            { 
                return new ErrorMessage
                {
                    Key = $"Mark of ComponentScoreId: {ComponentScoreId} of StudentId: {StudentId}",
                    Message = "This mark must be in range 0 to 10!"
                };
            }

            return null;
        }
    }
}
