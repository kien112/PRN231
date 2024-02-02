***REMOVED***using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Response;

namespace ScoreManagementApi.Core.Dtos.SubjectDto.Response
***REMOVED***
    public class SubjectResponse
    ***REMOVED***
        public int Id ***REMOVED*** get; set; ***REMOVED***
        public string Name ***REMOVED*** get; set; ***REMOVED*** = null!;
        public string? Description ***REMOVED*** get; set; ***REMOVED***
        public bool Active ***REMOVED*** get; set; ***REMOVED***
        public DateTime CreatedAt ***REMOVED*** get; set; ***REMOVED***
        public UserTiny? Creator ***REMOVED*** get; set; ***REMOVED***
        public List<ComponentScoreResponse>? ComponentScores ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
