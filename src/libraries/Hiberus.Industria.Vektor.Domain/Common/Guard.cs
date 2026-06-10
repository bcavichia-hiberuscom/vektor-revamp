using ErrorOr;

namespace Hiberus.Industria.Vektor.Domain.Common;

public static class Guard
{
    public static ErrorOr<string> NotNullOrWhiteSpace(
        string? value,
        string fieldName,
        string errorMessage
    )
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(fieldName, errorMessage);
        return value.Trim();
    }

    public static ErrorOr<Guid> NotEmpty(Guid value, string fieldName, string errorMessage)
    {
        if (value == Guid.Empty)
            return Error.Validation(fieldName, errorMessage);
        return value;
    }

    public static ErrorOr<string?> NullOrNotWhiteSpace(
        string? value,
        string fieldName,
        string errorMessage
    )
    {
        if (value is null)
            return (string?)null;
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(fieldName, errorMessage);
        return value.Trim();
    }

    public static ErrorOr<DateTime> DateNotBefore(
        DateTime value,
        DateTime minDate,
        string fieldName,
        string errorMessage
    )
    {
        if (value < minDate)
            return Error.Validation(fieldName, errorMessage);
        return value;
    }
}
