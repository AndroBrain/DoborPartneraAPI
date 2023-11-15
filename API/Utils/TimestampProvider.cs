namespace API.Utils
{
    public static class TimestampProvider
    {
        private static readonly DateTimeOffset _unixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToUnixTimeMicroseconds(this DateTimeOffset timestamp)
        {
            TimeSpan duration = timestamp - _unixEpoch;

            return duration.Ticks / 10;
        }
    }
}
