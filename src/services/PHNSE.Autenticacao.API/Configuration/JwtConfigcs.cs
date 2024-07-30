using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PHNSE.Autenticacao.API.Extensions;
using System.Text;

namespace PHNSE.Autenticacao.API.Configuration
{
    public static class JwtConfigcs
    {
        public static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //Variação para a passagem de parâmetros

            //Action<AuthenticationOptions> teste = options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //};

            //services.AddAuthentication(teste);

            var jwtConfig = configuration.GetSection("AppSetting");
            services.Configure<AppSettings>(jwtConfig);

            var settings = jwtConfig.Get<AppSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = settings.ValidoEm,
                    ValidIssuer = settings.Emissor
                };

            });

            return services;
        }

        public static IApplicationBuilder UseJwt(this IApplicationBuilder builder)
        {


            return builder;
        }
    }
}
