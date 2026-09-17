using System.Xml.Serialization;

namespace VendorBookingProcessor.Models.Booking;

public class VendorBooking
{
    [XmlElement("BookingId",  Namespace = "")]
     public string BookingId { get; set; } = string.Empty;

    [XmlElement("VendorId")]
    public string VendorId { get; set; } = string.Empty;

    [XmlElement("Customer")]
    public Customer Customer { get; set; } = new();

    [XmlElement("BookingDate")]
    public DateTime BookingDate { get; set; }

    [XmlElement("Amount")]
    public decimal Amount { get; set; }
}

public class Customer
{
    [XmlElement("FirstName")]
    public string FirstName { get; set; } = string.Empty;

    [XmlElement("LastName")]
    public string LastName { get; set; } = string.Empty;
}