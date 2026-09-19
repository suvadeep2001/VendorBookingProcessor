namespace VendorBookingProcessor.Models.Messaging;
using Newtonsoft.Json;

public class ProcessedMessage
{
    [JsonProperty("id")]
     public string Id { get; set; } = string.Empty;

    [JsonProperty("messageId")]
    public string MessageId { get; set; } = string.Empty;

    [JsonProperty("bookingId")]
    public string BookingId { get; set; } = string.Empty;

    [JsonProperty("correlationId")]
    public string CorrelationId { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

     [JsonProperty("processedAt")]
    public DateTime ProcessedAt { get; set; }
}