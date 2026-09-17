using System.Xml.Serialization;
using VendorBookingProcessor.Interfaces;
using VendorBookingProcessor.Models.Booking;

namespace VendorBookingProcessor.Services;


public class XmlParserService : IXmlParserService
{
    public VendorBooking Parse(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("Xml Payload cann't be empty", nameof(xml));
        }

          var root = new XmlRootAttribute("Booking")
        {
            Namespace = string.Empty
        };

        var serializer = new XmlSerializer(
            typeof(VendorBooking),
            root);

        using var reader = new StringReader(xml);
        var booking = serializer.Deserialize(reader) as VendorBooking;

        if (booking is null)
        {
            throw new InvalidOperationException(
                "Unable to deserialize booking XML.");
        }

        return booking;
    }
}