using ScoreManagementApi.Core.Dtos.ClassRoomDto;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Request;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Response;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User;

namespace ScoreManagementApi.Services
{
    public interface IClassService
    {
        Task<ResponseData<ClassResponse>> CreateClass(UserTiny? user, CreateClassRequest request);
        Task<ResponseData<ClassResponse>> CUDStudentsToClassRoom(UserTiny? userTiny, CUDStudentsToClass request);
        Task<ResponseData<ClassResponse>> GetClassRoomById(UserTiny? user, int id);
        Task<ResponseData<SearchList<ClassResponse>>> SearchClassRoom(UserTiny? user, SearchClassRoom request);
        Task<ResponseData<ClassResponse>> UpdateClassRoom(UserTiny? user, UpdateClassRequest request);
    }
}
