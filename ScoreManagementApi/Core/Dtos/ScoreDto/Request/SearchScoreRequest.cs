***REMOVED***using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;

namespace ScoreManagementApi.Core.Dtos.ScoreDto.Request
***REMOVED***
    public class SearchScoreRequest : SearchList<ScoreResponse>
    ***REMOVED***
        public string? StudentName ***REMOVED*** get; set; ***REMOVED***
        public int? ComponentScoreId ***REMOVED*** get; set; ***REMOVED***
        public int? ClassId ***REMOVED*** get; set; ***REMOVED***

        public new void ValidateInput()
        ***REMOVED***
            base.ValidateInput();

            if(!String.IsNullOrEmpty(StudentName))
            ***REMOVED***
                StudentName = StudentName.Trim().ToLower();
    ***REMOVED***

***REMOVED***
***REMOVED***
***REMOVED***
