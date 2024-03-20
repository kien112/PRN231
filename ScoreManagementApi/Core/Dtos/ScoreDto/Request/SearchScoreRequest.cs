using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;
using ScoreManagementApi.Core.OtherObjects;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
{
    public class SearchScoreRequest
    {
        public string? StudentName { get; set; }
        public int? ComponentScoreId { get; set; }
        public int? ClassId { get; set; }
        public int? TotalElements { get; set; }
        public string? OrderBy { get; set; }
        public string? SortBy { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public void ValidateInput()
        {
            if(!String.IsNullOrEmpty(StudentName))
            {
                StudentName = StudentName.Trim().ToLower();
            }

            if (OrderBy == null || (!OrderBy.Equals(StaticString.ASC) && !OrderBy.Equals(StaticString.DESC)))
            {
                OrderBy = StaticString.ASC;
            }

            if (PageIndex == null || PageIndex < 0)
            {
                PageIndex = 0;
            }

            if (PageSize == null || PageSize <= 0)
            {
                PageSize = 9999;
            }
        }
    }
}
