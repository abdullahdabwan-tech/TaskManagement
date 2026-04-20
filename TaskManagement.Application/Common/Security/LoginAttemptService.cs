using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Common.Security
{
    public class LoginAttemptService
    {
        private static Dictionary<string, (int count, DateTime lastAttempt)> _attempts = new();

        public bool IsBlocked(string key)
        {
            if (!_attempts.ContainsKey(key)) return false;

            var (count, last) = _attempts[key];

            return count >= 5 && DateTime.UtcNow - last < TimeSpan.FromMinutes(5);
        }

        public void RegisterFailure(string key)
        {
            if (!_attempts.ContainsKey(key))
                _attempts[key] = (1, DateTime.UtcNow);
            else
            {
                var (count, _) = _attempts[key];
                _attempts[key] = (count + 1, DateTime.UtcNow);
            }
        }

        public void Reset(string key)
        {
            _attempts.Remove(key);
        }
    }

}
