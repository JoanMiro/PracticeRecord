namespace PracticeRecord.Models
{
    using System;

    /// <summary>
    /// Used for getting DateTime.Now(), time is changeable for unit testing
    /// </summary>
    public static class SystemTime
    {
        private static Func<DateTime> _defaultDateTimeNow = () => DateTime.Now;
        private static Func<DateTime> _defaultDateTimeUtcNow = () => DateTime.UtcNow;
        private static Func<DateTime> _defaultDateTimeToday = () => DateTime.Today;

        /// <summary>
        /// Normally this is a pass-through to <see cref="DateTime.Now"/>, but it can be overridden with <see cref="SetDateTime"/> for testing or debugging.
        /// </summary>
        public static DateTime Now => _defaultDateTimeNow();
        
        /// <summary>
        /// Normally this is a pass-through to <see cref="DateTime.UtcNow"/>, but it can be overridden with <see cref="SetDateTime"/> for testing or debugging.
        /// </summary>
        public static DateTime UtcNow => _defaultDateTimeUtcNow();

        /// <summary>
        /// Normally this is a pass-through to <see cref="DateTime.Today"/>, but it can be overridden with <see cref="SetDateTime"/> for testing or debugging.
        /// </summary>
        public static DateTime Today => _defaultDateTimeToday();

        /// <summary>
        /// Set time to return when <see cref="Now"/> is called.
        /// </summary>
        public static void SetDateTime(DateTime dateTimeNow)
        {
            _defaultDateTimeNow = () => dateTimeNow;
            _defaultDateTimeUtcNow = () => dateTimeNow;
            _defaultDateTimeToday = () => dateTimeNow;
        }

        /// <summary>
        /// Resets <see cref="Now"/> to return <see cref="DateTime.Now"/>.
        /// </summary>
        public static void ResetDateTime()
        {
            _defaultDateTimeNow = () => DateTime.Now;
            _defaultDateTimeUtcNow = () => DateTime.UtcNow;
        }
    }
}
