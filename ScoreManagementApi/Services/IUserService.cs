using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Request;
using ScoreManagementApi.Core.Dtos.User.Response;

namespace ScoreManagementApi.Services
{
    public interface IUserService
    {
        Task<ResponseData<SearchList<UserResponse>>> GetUsers(SearchUsers request);
        Task<ResponseData<UserResponse>> CreateUser(CreateUserRequest request);
        Task<ResponseData<UserResponse>> UpdateUser(UpdateUserRequest request);
        Task<ResponseData<UserResponse>> GetUserById(string Id);
        Task<ResponseData<LoginResponse>> Login(LoginRequest request);
        Task<ResponseData<UpdateUserRole>> UpdateUserRole(UpdateUserRole request);
        Task<ResponseData<string>> ForgotPassword(ForgotPasswordRequest request);
    }
}
