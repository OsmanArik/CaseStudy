namespace System
{
    public static class DatetimeExtensions
    {
        public static string DateTimeFormatEx(this DateTime date)
            => date.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}