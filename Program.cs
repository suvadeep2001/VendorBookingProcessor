using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VendorBookingProcessor.Services;
using VendorBookingProcessor.Interfaces;
using VendorBookingProcessor.Validators;
using Microsoft.Azure.Cosmos;
using Azure.Identity;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var cosmosEndpoint =
    builder.Configuration["CosmosDb:AccountEndpoint"]
    ?? throw new InvalidOperationException(
        "CosmosDb:AccountEndpoint is missing.");

        builder.Services.AddSingleton<CosmosClient>(_ =>
{
    return new CosmosClient(
        cosmosEndpoint,
        new DefaultAzureCredential());
});

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddSingleton<IXmlParserService, XmlParserService>()
    .AddSingleton<BookingValidator>()
    .AddSingleton<ICosmosRepository,CosmosRepository>();

builder.Build().Run();