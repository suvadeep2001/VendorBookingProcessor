using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VendorBookingProcessor.Interfaces;
using VendorBookingProcessor.Services;

namespace VendorBookingProcessor.Functions;

public class VendorBookingFunction
{
    private readonly ILogger<VendorBookingFunction> _logger;
    private readonly IXmlParserService _xmlParser;

    public VendorBookingFunction(ILogger<VendorBookingFunction> logger, IXmlParserService xmlParser)
    {
        _logger = logger;
        _xmlParser = xmlParser;
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


        var booking = _xmlParser.Parse(xml) ;

        _logger.LogInformation("Booking parsed successfully. BookingId: {BookingId}, VendorId: {VendorId}",
            booking.BookingId,
            booking.VendorId);

        await Task.CompletedTask;
    }

}