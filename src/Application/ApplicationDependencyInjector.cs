using Application.Beneficiary;
using Application.Payment.Topup;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application
{
    public static class ApplicationDependencyInjector
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ITopupApp, TopupApp>();
            services.AddScoped<IBeneficiaryApp, BeneficiaryApp>();

            services.AddPersistenceDependencies(connectionString);

            return services;
        }
    }
}
