using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shaesk.Auth.Data;
using Shaesk.Auth.Models;
using Shaesk.Auth.Services;
using System;
using System.Text;

namespace Shaesk.Auth
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Bu modül için 
        /// app.UseAuthentication();
        /// app.UseApiResponseAndExceptionWrapper();
        /// aktif etmeniz gerekiyor.
        /// </summary>
        /// <param name="authSettings">Auth ayarlarını yollamak gerekiyor </param>
        public static void AddAuthLayer(
            this IServiceCollection services,
            AuthSettings authSettings,
            Action<DbContextOptionsBuilder> authDbOptionsBuilder)
        {
            services.AddDbContext<AuthDbContext>(authDbOptionsBuilder);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(
                                authSettings.SigningKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddTransient<IAuthService, AuthService>();

        }
    }
}
