using Application.Infrastructure;
using Common.Configuration;
using Infrastructure.FinPayBalanceService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjector
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBalanceService, BalanceService>();

            // Binding configuration settings from appsettings
            FinPayBalanceServiceSettings finPayBalanceServiceSettings = new FinPayBalanceServiceSettings();
            configuration.GetSection(FinPayBalanceServiceSettings.SettingName).Bind(finPayBalanceServiceSettings);
            services.AddSingleton(finPayBalanceServiceSettings);

            return services;
        }
    }
}
