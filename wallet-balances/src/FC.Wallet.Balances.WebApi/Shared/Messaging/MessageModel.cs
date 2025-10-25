namespace FC.Wallet.Balances.WebApi.Shared.Messaging;

internal sealed record MessageModel<T>(string Name, T Payload);