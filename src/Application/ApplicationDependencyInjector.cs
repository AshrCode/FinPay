using Application.Payment.Topup;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjector
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITopupApp, TopupApp>();

            //services.AddPersistenceDependencies();

            return services;
        }
    }
}
