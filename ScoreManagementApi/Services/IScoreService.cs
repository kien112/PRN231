using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Services
{
    public interface IScoreService
    {
        Task<ResponseData<ScoreResponse>> CUDScore(UserTiny? user, List<CUDScoreRequest> request);
        Task<ResponseData<ExportScoreResponse>> ExportScore(UserTiny? user, int? classId);
        Task<ResponseData<ScoreResponse>> ImportScore(UserTiny? userTiny, ImportScoresRequest request);
        Task<ResponseData<SearchList<ScoreResponse>>> SearchScore(UserTiny? user, SearchScoreRequest request);
    }
}
