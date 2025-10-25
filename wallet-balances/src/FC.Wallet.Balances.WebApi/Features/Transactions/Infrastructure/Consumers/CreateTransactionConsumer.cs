using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using FC.Wallet.Balances.WebApi.Features.Transactions.Commands.CreateTransaction;
using FC.Wallet.Balances.WebApi.Shared.Messaging;

namespace FC.Wallet.Balances.WebApi.Features.Transactions.Infrastructure.Consumers;

internal sealed class CreateTransactionConsumer(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = new()
    {
        BootstrapServers = "kafka:29092",
        GroupId = "create-balance-consumer-group",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = true,
        EnableAutoOffsetStore = false,
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe("transactions");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = await Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);

            if (consumeResult is null || consumeResult.IsPartitionEOF || stoppingToken.IsCancellationRequested)
                continue;

            var @event = JsonSerializer.Deserialize<MessageModel<TransactionCreatedEvent>>(consumeResult.Message.Value);
            if (@event?.Payload is null)
                continue;

            var scope = serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<CreateTransactionHandler>();
            var command = new CreateTransactionCommand(
                @event.Payload.Id,
                @event.Payload.AccountIdFrom,
                @event.Payload.AccountIdTo,
                @event.Payload.Amount
            );
            await handler.HandleAsync(command, stoppingToken);
            consumer.StoreOffset(consumeResult);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}

internal sealed record TransactionCreatedEvent(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("account_id_from")]
    Guid AccountIdFrom,
    [property: JsonPropertyName("account_id_to")]
    Guid AccountIdTo,
    [property: JsonPropertyName("amount")] decimal Amount
);