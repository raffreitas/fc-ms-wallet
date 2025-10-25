using FC.Wallet.Balances.WebApi.Features.Transactions.Domain;

namespace FC.Wallet.Balances.WebApi.Features.Transactions.Commands.CreateTransaction;

internal sealed class CreateTransactionHandler(ITransactionRepository transactionRepository)
{
    public async Task HandleAsync(CreateTransactionCommand command, CancellationToken ct = default)
    {
        var transaction = new Transaction(command.Id, command.AccountIdFrom, command.AccountIdTo, command.Amount);
        await transactionRepository.Save(transaction, ct);
    }
}