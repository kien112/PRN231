***REMOVED***using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
***REMOVED***
    public class ImportScoresRequest
    ***REMOVED***
        public IFormFile? ExcelFile ***REMOVED*** get; set; ***REMOVED***
        public int? ClassId ***REMOVED*** get; set; ***REMOVED***

        public List<ErrorMessage> ValidateInput()
        ***REMOVED***
            var errors = new List<ErrorMessage>();

            if (ExcelFile == null || ExcelFile.Length <= 0)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Excel File",
                    Message = "File is required!"
        ***REMOVED***);
            else if (!ExcelFile.FileName.EndsWith(".xlsx"))
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "Excel File",
                    Message = "InValid file format!"
        ***REMOVED***);

            if (ClassId == null)
                errors.Add(new ErrorMessage
                ***REMOVED***
                    Key = "ClassId",
                    Message = "ClassId is required!"
        ***REMOVED***);

            return errors;
***REMOVED***
***REMOVED***
***REMOVED***
