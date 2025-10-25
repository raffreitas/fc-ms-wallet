namespace FC.Wallet.Balances.WebApi.Features.Transactions.Domain;

internal sealed class Transaction(Guid id, Guid accountIdFrom, Guid accountIdTo, decimal amount)
{
    public Guid Id { get; } = id;
    public Guid AccountIdFrom { get; } = accountIdFrom;
    public Guid AccountIdTo { get; } = accountIdTo;
    public decimal Amount { get; } = amount;
}