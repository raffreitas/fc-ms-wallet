using FC.Wallet.Balances.WebApi.Shared.Persistence;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace FC.Wallet.Balances.WebApi.Shared;

internal static class SharedFeature
{
    public static IServiceCollection AddSharedFeature(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddPersistence(configuration);
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        const string connectionName = "Database";
        var connectionString = configuration.GetConnectionString(connectionName) ??
                               throw new InvalidOperationException(
                                   $"The connection string {connectionName} was not found");

        var ngpsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.TryAddSingleton(ngpsqlDataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        return services;
    }
}