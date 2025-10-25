namespace FC.Wallet.Balances.WebApi.Features.Transactions.Domain;

internal interface ITransactionRepository
{
    Task Save(Transaction transaction, CancellationToken cancellationToken = default);
}