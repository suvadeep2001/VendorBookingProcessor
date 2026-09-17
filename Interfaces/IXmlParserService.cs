using VendorBookingProcessor.Models.Booking;

namespace VendorBookingProcessor.Interfaces;


public interface IXmlParserService
{
    VendorBooking Parse(string xml);
}