using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Security.RateLimiting
{
    public class UserRateLimiter
    {
        private class LimitInfo
        {
            public int Count;
            public DateTime WindowStart;
        }

        private readonly ConcurrentDictionary<string, LimitInfo> _ipLimits = new();

        private readonly int _limit = 20;
        private readonly TimeSpan _window = TimeSpan.FromMinutes(1);

        public bool IsLimited(string key)
        {
            var now = DateTime.UtcNow;

            var entry = _ipLimits.GetOrAdd(key, _ => new LimitInfo
            {
                Count = 0,
                WindowStart = now
            });

            // reset window
            if (now - entry.WindowStart > _window)
            {
                entry.Count = 0;
                entry.WindowStart = now;
            }

            entry.Count++;

            return entry.Count > _limit;
        }
    }
}
