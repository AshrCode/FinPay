using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Beneficiary;
using Persistence.DatabaseSchema;
using Persistence.Transaction;
using Persistence.User;

namespace Persistence
{
    public static class PersistenceDependencyInjector
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FinPayDbContext>(options => options.UseSqlServer(connectionString));


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeneficiararyRepository, BeneficiararyRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
