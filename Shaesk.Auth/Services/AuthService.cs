using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shaesk.Auth.Data;
using Shaesk.Auth.Dtos;
using Shaesk.Auth.Entities;
using Shaesk.Auth.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shaesk.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly AuthSettings _authSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AuthDbContext context,
            ILogger<AuthService> logger,
            IOptions<AuthSettings> authSettings)
        {
            _context = context;
            _logger = logger;
            _authSettings = authSettings.Value;
        }

        public async Task<ApiResponse> CreateUser(CreateUserRequestModel req)
        {
            User user = new();
            user.Email = req.Email;
            user.Username = req.Username;
            user.IsActive = true;
            user.IsEmailVerification = false;

            if (req.IsHash)
                user.PasswordHash = req.Password;
            else
                user.Password = req.Password;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new ApiResponse(
                "Kullanıcı başarı ile oluşturuldu.",
                new CreateUserResponseModel
                {
                    Email = user.Email,
                    Id = user.Id
                });
        }

        public async Task<ApiResponse> Login(LoginRequestModel req)
        {
            User user = await _context.Users.FirstOrDefaultAsync(
                s => s.Email.ToLower() == req.LoginKey || s.Username.ToLower() == req.LoginKey);

            if (user is null)
                throw new ApiException($"{req.LoginKey} kullanıcısı bulunamadı.");

            if (!user.IsActive)
                throw new ApiException($"Hesabınız aktif değil lütfen sistem yöneticinize başvurunuz.");

            if (!user.IsEmailVerification)
                throw new ApiException($"Lütfen önce mail adresinizi doğrulayınız.");

            user = await _context.Users.FirstOrDefaultAsync(
                s => s.Email == user.Email
                && s.Password == req.Password || s.PasswordHash == req.Password);

            if (user is null)
                throw new ApiException($"Kullanıcı veya parola hatalı.");


            return new ApiResponse(
                "Giriş başarılı hoş geldiniz.",
                new LoginResponseModel
                {
                    Email = user.Email,
                    Role = user.Role,
                    Token = CreateToken(user)
                });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.SerialNumber,user.ParentId.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authSettings.SigningKey)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
