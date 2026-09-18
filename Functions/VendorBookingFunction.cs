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

    public VendorBookingFunction(ILogger<VendorBookingFunction> logger, IXmlParserService xmlParser, BookingValidator bookingValidator)
    {
        _logger = logger;
        _xmlParser = xmlParser;
        _validator = bookingValidator;
    }

    [Function("VendorBookingFunction")]
    public async Task Run(
        [ServiceBusTrigger(
            "vendor-booking-in",
            Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message)
    {
        _logger.LogInformation("Message received MessageId:{MessageId}", message.MessageId);

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

        await Task.CompletedTask;
    }

}