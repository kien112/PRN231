using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.ScoreDto.Response;

namespace ScoreManagementClient.Dtos.ScoreDto.Request
{
    public class SearchScoreRequest : SearchList<ScoreResponse>
    {
        public string? StudentName { get; set; }
        public int? ComponentScoreId { get; set; }
        public int? ClassId { get; set; }

        public new void ValidateInput()
        {
            base.ValidateInput();

            if(!String.IsNullOrEmpty(StudentName))
            {
                StudentName = StudentName.Trim().ToLower();
            }

        }
    }
}
