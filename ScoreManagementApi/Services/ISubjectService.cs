***REMOVED***using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.SubjectDto.Request;
using ScoreManagementApi.Core.Dtos.SubjectDto.Response;
using ScoreManagementApi.Core.Dtos.User;

***REMOVED***
***REMOVED***
    public interface ISubjectService
    ***REMOVED***
        Task<ResponseData<SubjectResponse>> CreateSubject(UserTiny? user, CreateSubjectRequest request);
        Task<ResponseData<SubjectResponse>> GetSubjectById(UserTiny? userTiny, int? id);
        Task<ResponseData<SearchList<SubjectResponse>>> SearchSubjects(UserTiny? userTiny, SearchSubject request);
        Task<ResponseData<SubjectResponse>> UpdateSubject(UserTiny? userTiny, UpdateSubjectRequest request);
***REMOVED***
***REMOVED***
