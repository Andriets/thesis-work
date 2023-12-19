using AutoTrader.Domain.Abstractions;
using AutoTrader.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AutoTrader.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration
                    .GetConnectionString("DefaultConnection")!),
                    contextLifetime: ServiceLifetime.Transient,
                    optionsLifetime: ServiceLifetime.Singleton);

            services.AddTransient<IAppDbContext>(provider => provider.GetService<AppDbContext>()!);

            services.AddIdentity<AppUser, Role>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserStore<UserStore<AppUser, Role, AppDbContext, Guid>>()
                .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>()
                .AddDefaultTokenProviders();

            services
                .AddMemoryCache()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTOptions:SecretKey"))),

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5),
                    };
                });

            return services;
        }
    }
}
