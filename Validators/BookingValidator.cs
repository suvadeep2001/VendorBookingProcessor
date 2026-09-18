using VendorBookingProcessor.Models.Booking;
using VendorBookingProcessor.Models.Results;

namespace VendorBookingProcessor.Validators;

public class BookingValidator
{
    public ValidationResult validate(VendorBooking vendorBooking)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(vendorBooking.BookingId))
            errors.Add("BookingId is required.");

        if (string.IsNullOrWhiteSpace(vendorBooking.VendorId))
            errors.Add("VendorId is required.");

        if (vendorBooking.Customer is null)
        {
            errors.Add("customer is required");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(vendorBooking.Customer.FirstName))
            {
                errors.Add("First name is required......");
            }
            if (string.IsNullOrWhiteSpace(vendorBooking.Customer.LastName))
            {
                errors.Add("Last name is required......");
            }
        }

        if (vendorBooking.Amount < 0)
        {
            errors.Add("Amount can't be less than 0");
        }

        if (vendorBooking.BookingDate == default)
        {
            errors.Add("BookingDate is required.");
        }

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors.ToArray());
    }
}