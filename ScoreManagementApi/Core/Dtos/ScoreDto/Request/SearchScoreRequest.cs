using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
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
