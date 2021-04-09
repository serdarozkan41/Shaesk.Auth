using AutoWrapper.Wrappers;
using Shaesk.Auth.Dtos;
using System.Threading.Tasks;

namespace Shaesk.Auth.Services
{
    public interface IAuthService
    {
        Task<ApiResponse> CreateUser(CreateUserRequestModel req);
        Task<ApiResponse> Login(LoginRequestModel req);
    }
}
