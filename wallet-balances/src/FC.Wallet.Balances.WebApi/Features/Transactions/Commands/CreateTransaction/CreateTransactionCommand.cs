namespace FC.Wallet.Balances.WebApi.Features.Transactions.Commands.CreateTransaction;

internal readonly record struct CreateTransactionCommand(
    Guid Id,
    Guid AccountIdFrom,
    Guid AccountIdTo,
    decimal Amount
);