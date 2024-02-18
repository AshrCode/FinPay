using Microsoft.Extensions.DependencyInjection;
using Persistence.Beneficiary;
using Persistence.Transaction;
using Persistence.User;

namespace Persistence
{
    public static class PersistenceDependencyInjector
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeneficiararyRepository, BeneficiararyRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
