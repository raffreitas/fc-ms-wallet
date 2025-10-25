using FC.Wallet.Balances.WebApi.Features.Transactions.Commands.CreateTransaction;
using FC.Wallet.Balances.WebApi.Features.Transactions.Domain;
using FC.Wallet.Balances.WebApi.Features.Transactions.Infrastructure.Consumers;
using FC.Wallet.Balances.WebApi.Features.Transactions.Infrastructure.Repositories;
using FC.Wallet.Balances.WebApi.Features.Transactions.Queries;

namespace FC.Wallet.Balances.WebApi.Features.Transactions;

internal static class TransactionsFeature
{
    public static IServiceCollection AddTransactionsFeature(this IServiceCollection services)
    {
        services.AddScoped<CreateTransactionHandler>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddHostedService<CreateTransactionConsumer>();

        return services;
    }

    public static WebApplication UseTransactionsFeature(this WebApplication app)
    {
        app.MapGetBalancesEndpoint();
        return app;
    }
}