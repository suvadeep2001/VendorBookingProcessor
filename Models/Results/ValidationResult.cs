namespace VendorBookingProcessor.Models.Results;

public class ValidationResult
{
    public bool IsValid { get; init; }

    public List<string> Errors { get; init; } = [];

    public static ValidationResult Success()
    {
        return new ValidationResult
        {
            IsValid = true
        };
    }

    public static ValidationResult Failure(
        params string[] errors)
    {
        return new ValidationResult
        {
            IsValid = false,
            Errors = errors.ToList()
        };
    }
}