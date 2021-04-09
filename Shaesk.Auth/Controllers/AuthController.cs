using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaesk.Auth.Dtos;
using Shaesk.Auth.Services;
using System.Threading.Tasks;

namespace Shaesk.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("CreateUser")]
        public async Task<ApiResponse> CreateUser(CreateUserRequestModel req)
        {
            return await _authService.CreateUser(req);
        }

        [HttpPost("Login")]
        public async Task<ApiResponse> Login(LoginRequestModel req)
        {
            return await _authService.Login(req);
        }
    }
}
