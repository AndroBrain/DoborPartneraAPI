using System.Collections.Concurrent;

namespace API.Utils.limiter
{
    public interface ILoginLimiter
    {
        public bool CanLogin(string email);
        public void ResetAttempts(string email);
        public void AddAttempt(string email);
    }
    public class LoginLimiter : ILoginLimiter
    {
        private static readonly ConcurrentDictionary<string, LoginLimit> _loginLimit = new ConcurrentDictionary<string, LoginLimit>();

        public bool CanLogin(string email)
        {
            var lastLimit = _loginLimit.GetValueOrDefault(email);
            var attempts = lastLimit?.Attempts ?? 0;
            var dateTime = DateTime.UtcNow;
            if (lastLimit?.LastDateTime?.AddMinutes(10) < dateTime)
            {
                attempts = 0;
            }
            return attempts < 3;
        }

        public void ResetAttempts(string email)
        {
            _loginLimit[email] = new LoginLimit()
            {
                Attempts = 0,
                LastDateTime = null
            };
        }

        public void AddAttempt(string email)
        {
            var lastLimit = _loginLimit.GetValueOrDefault(email);
            var attempts = lastLimit?.Attempts ?? 0;
            var dateTime = DateTime.UtcNow;
            if (lastLimit?.LastDateTime?.AddMinutes(10) < dateTime)
            {
                attempts = 0;
            }
            _loginLimit[email] = new LoginLimit()
            {
                Attempts = attempts + 1,
                LastDateTime = DateTime.UtcNow,
            };
        }
    }
}
