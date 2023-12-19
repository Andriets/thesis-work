using AutoTrader.Application.Bots;
using AutoTrader.Application.Dtos;
using Binance.Net;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrader.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssemblies(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddBinance();

            services.AddSingleton<IBotService, BotService>();

            services.Configure<JwtOptions>(configuration.GetSection("JWTOptions"));

            return services;
        }
    }
}
