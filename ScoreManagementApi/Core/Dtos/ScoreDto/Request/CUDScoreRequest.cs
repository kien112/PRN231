***REMOVED***using ScoreManagementApi.Core.Dtos.Common;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
***REMOVED***
    public class CUDScoreRequest
    ***REMOVED***
        public float? Mark ***REMOVED*** get; set; ***REMOVED***
        public string? StudentId ***REMOVED*** get; set; ***REMOVED***
        public int? ComponentScoreId ***REMOVED*** get; set; ***REMOVED***

        public ErrorMessage? ValidateInput()
        ***REMOVED***
            if(StudentId == null ||  ComponentScoreId == null)
            ***REMOVED***
                return new ErrorMessage
                ***REMOVED***
                    Key = "StudentId and ComponentScoreId",
                    Message = "StudentId and ComponentScoreId is required!"
    ***REMOVED***
    ***REMOVED***
            else if(Mark != null && (Mark < 0 || Mark > 10))
            ***REMOVED*** 
                return new ErrorMessage
                ***REMOVED***
                    Key = $"Mark of ComponentScoreId: ***REMOVED***ComponentScoreId***REMOVED*** of StudentId: ***REMOVED***StudentId***REMOVED***",
                    Message = "This mark must be in range 0 to 10!"
    ***REMOVED***
    ***REMOVED***

            return null;
***REMOVED***
***REMOVED***
***REMOVED***
