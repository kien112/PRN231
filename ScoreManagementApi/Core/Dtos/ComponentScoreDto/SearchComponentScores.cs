***REMOVED***using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;

namespace ScoreManagementApi.Core.Dtos.ComponentScoreDto
***REMOVED***
    public class SearchComponentScores : SearchList<ComponentScoreResponse>
    ***REMOVED***
        public string? Name ***REMOVED*** get; set; ***REMOVED***
        public float? Percent ***REMOVED*** get; set; ***REMOVED***
        public bool? Active ***REMOVED*** get; set; ***REMOVED***
        public int? SubjectId ***REMOVED*** get; set; ***REMOVED***

        public new void ValidateInput()
        ***REMOVED***
            base.ValidateInput();

            if(!String.IsNullOrEmpty(Name))
                Name = Name.Trim().ToLower();
            
            if (Active == null)
                Active = false;

***REMOVED***
***REMOVED***
***REMOVED***
