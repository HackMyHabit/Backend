using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using HackMyHabit.Infrastructure.Caching;
using HackMyHabit.Domain.Commons.Abstractions;

namespace HackMyHabit.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddSharedInsfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetValue<string>("ConnectionString");
            var redis = configuration.GetValue<string>("RedisConnectionString");
            var multiplexer = ConnectionMultiplexer.Connect(redis!);

            return services
                .AddSingleton<IConnectionMultiplexer>(multiplexer)
                .AddSingleton<ICacheService, CacheService>()
                .AddSingleton(x => x.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        }
    }
}

