using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Services
{
    public interface IComponentScoreService
    {
        Task<ResponseData<ComponentScoreResponse>> CreateComponentScore(UserTiny? user, CreateComponentScoreRequest request);
        Task<ResponseData<int?>> DeleteComponentScore(UserTiny? userTiny, int id);
        Task<ResponseData<ComponentScoreResponse>> GetComponentScoreById(UserTiny? user, int id);
        Task<ResponseData<SearchList<ComponentScoreResponse>>> SearchComponentScore(UserTiny? user, SearchComponentScores request);
        Task<ResponseData<ComponentScoreResponse>> UpdateComponentScore(UserTiny? userTiny, UpdateComponentScoreRequest request);
    }
}
