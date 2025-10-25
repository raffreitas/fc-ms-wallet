using System.Data.Common;

namespace FC.Wallet.Balances.WebApi.Shared.Persistence;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}