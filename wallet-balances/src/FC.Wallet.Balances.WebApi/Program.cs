using FC.Wallet.Balances.WebApi.Features.Transactions;
using FC.Wallet.Balances.WebApi.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSharedFeature(builder.Configuration);
builder.Services.AddTransactionsFeature();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseTransactionsFeature();

await app.RunAsync();