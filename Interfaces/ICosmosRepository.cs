using VendorBookingProcessor.Models.Messaging;

namespace VendorBookingProcessor.Interfaces;

public interface ICosmosRepository
{
    // Stores the processing record in Cosmos DB.
    Task<bool> TryCreateProcessingRecordAsync(ProcessedMessage processedMessage);
}