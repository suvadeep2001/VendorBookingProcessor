using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VendorBookingProcessor.Interfaces;
using VendorBookingProcessor.Services;
using VendorBookingProcessor.Validators;

namespace VendorBookingProcessor.Functions;

public class VendorBookingFunction
{
    private readonly ILogger<VendorBookingFunction> _logger;
    private readonly IXmlParserService _xmlParser;
    private readonly BookingValidator _validator;
    private readonly ICosmosRepository _cosmosRepository;

    public VendorBookingFunction(ILogger<VendorBookingFunction> logger, IXmlParserService xmlParser, BookingValidator bookingValidator, ICosmosRepository cosmosRepository)
    {
        _logger = logger;
        _xmlParser = xmlParser;
        _validator = bookingValidator;
        _cosmosRepository = cosmosRepository;
    }

    [Function("VendorBookingFunction")]
    public async Task Run(
        [ServiceBusTrigger(
            "vendor-booking-in",
            Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message)
    {
        var messageId = message.MessageId;
        _logger.LogInformation("Message received MessageId:{MessageId}", messageId);

        var xml = message.Body.ToString();
        _logger.LogInformation("Message body:{Body}", xml);


        var booking = _xmlParser.Parse(xml);

        var ValidationResult = _validator.validate(booking);

        if (!ValidationResult.IsValid)
        {
            _logger.LogWarning(
        "Booking validation failed. MessageId: {MessageId}, Errors: {Errors}",
        message.MessageId,
        string.Join(" | ", ValidationResult.Errors));

            return;
        }

        _logger.LogInformation("Booking parsed successfully. BookingId: {BookingId}, VendorId: {VendorId}",
            booking.BookingId,
            booking.VendorId);

        var processedMessage = new Models.Messaging.ProcessedMessage
        {
            Id = messageId,
            MessageId = messageId,

            BookingId = booking.BookingId,
            CorrelationId =
                message.CorrelationId ?? Guid.NewGuid().ToString(),
            Status = "Processed",

            ProcessedAt = DateTime.UtcNow

        };
         var isNewMessage =
            await _cosmosRepository
                .TryCreateProcessingRecordAsync(
                    processedMessage);

         if (!isNewMessage)
        {
            _logger.LogWarning(
                "Duplicate message detected. " +
                "MessageId: {MessageId}, BookingId: {BookingId}",
                messageId,
                booking.BookingId);

            return;
        }

        _logger.LogInformation(
            "Message processing record saved to Cosmos. MessageId: {MessageId}",
            messageId);

            await Task.CompletedTask;
    }

}