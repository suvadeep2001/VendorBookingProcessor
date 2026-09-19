using Microsoft.Azure.Cosmos;
using VendorBookingProcessor.Models.Messaging;
using VendorBookingProcessor.Interfaces;
using Microsoft.Extensions.Configuration;

namespace VendorBookingProcessor.Services;

public class CosmosRepository : ICosmosRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public CosmosRepository(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;

        var databaseName =
            configuration["CosmosDb:DatabaseName"]
            ?? throw new InvalidOperationException(
                "CosmosDb:DatabaseName is missing.");

        var containerName =
            configuration["CosmosDb:ContainerName"]
            ?? throw new InvalidOperationException(
                "CosmosDb:ContainerName is missing.");

        // GetContainer does not create the container.
        // It simply creates a reference to the existing container.
        _container = _cosmosClient.GetContainer(
            databaseName,
            containerName);
    }

    public async Task<bool> TryCreateProcessingRecordAsync(
        ProcessedMessage processedMessage)
    {
        await _container.CreateItemAsync(
                processedMessage,
                new PartitionKey(
                    processedMessage.MessageId));
                    
            return true;
    }
}