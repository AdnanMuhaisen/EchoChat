namespace EchoChat.Domain.Common;

public static class DateTimeProvider
{
    public static readonly string JordanStandardTime = "Jordan Standard Time";

    public static DateTime JordanDateTimeNow => ToJordanDateTime(DateTime.UtcNow);

    public static DateTime ToJordanDateTime(this DateTime dateTime)
    {
        var jordanTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(JordanStandardTime);
        var jordanDateTime = TimeZoneInfo.ConvertTime(dateTime, jordanTimeZoneInfo);

        return jordanDateTime;
    }
}