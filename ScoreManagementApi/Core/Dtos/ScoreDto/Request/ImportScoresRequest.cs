using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
{
    public class ImportScoresRequest
    {
        public IFormFile? ExcelFile { get; set; }
        public int? ClassId { get; set; }

        public List<ErrorMessage> ValidateInput()
        {
            var errors = new List<ErrorMessage>();

            if (ExcelFile == null || ExcelFile.Length <= 0)
                errors.Add(new ErrorMessage
                {
                    Key = "Excel File",
                    Message = "File is required!"
                });
            else if (!ExcelFile.FileName.EndsWith(".xlsx"))
                errors.Add(new ErrorMessage
                {
                    Key = "Excel File",
                    Message = "InValid file format!"
                });

            if (ClassId == null)
                errors.Add(new ErrorMessage
                {
                    Key = "ClassId",
                    Message = "ClassId is required!"
                });

            return errors;
        }
    }
}
