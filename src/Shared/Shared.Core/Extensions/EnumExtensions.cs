namespace System
{
    public static class EnumExtensions
    {
        public static int ToIntEx(this Enum enumValue) => (int)((object)enumValue);
    }
}