using Dapper;
using FC.Wallet.Balances.WebApi.Shared.Persistence;

namespace FC.Wallet.Balances.WebApi.Features.Transactions.Queries;

internal static class GetBalances
{
    public static IEndpointRouteBuilder MapGetBalancesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/balances/{accountId:guid}",
                async (Guid accountId, IDbConnectionFactory dbConnectionFactory, CancellationToken ct) =>
                {
                    const string query =
                        """
                        with received as (select coalesce(sum(amount), 0) as amount
                                          from public.transactions
                                          where account_id_to = @AccountId),
                             sent as (select coalesce(sum(amount), 0) as amount
                                      from public.transactions
                                      where account_id_from = @AccountId)

                        select (r.amount - s.amount)
                        from received r,
                             sent s
                        """;

                    await using var connection = await dbConnectionFactory.OpenConnectionAsync(ct);
                    var balance = await connection.QuerySingleAsync<decimal>(query, new { AccountId = accountId });
                    return Results.Ok(new { AccountId = accountId, Balance = balance });
                })
            .WithTags("Balances")
            .WithName("GetBalances")
            .WithOpenApi();

        return endpoints;
    }
}