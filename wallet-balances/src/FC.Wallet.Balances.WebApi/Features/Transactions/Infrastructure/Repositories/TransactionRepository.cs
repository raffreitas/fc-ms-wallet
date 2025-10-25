using Dapper;
using FC.Wallet.Balances.WebApi.Features.Transactions.Domain;
using FC.Wallet.Balances.WebApi.Shared.Persistence;

namespace FC.Wallet.Balances.WebApi.Features.Transactions.Infrastructure.Repositories;

internal sealed class TransactionRepository(IDbConnectionFactory dbConnectionFactory) : ITransactionRepository
{
    public async Task Save(Transaction transaction, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            insert into public.transactions (id, account_id_from, account_id_to, amount)
            values (@Id, @AccountIdFrom, @AccountIdTo, @Amount)
            """;

        await using var connection = await dbConnectionFactory.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(sql, new
        {
            transaction.Id,
            transaction.AccountIdFrom,
            transaction.AccountIdTo,
            transaction.Amount
        });
    }
}