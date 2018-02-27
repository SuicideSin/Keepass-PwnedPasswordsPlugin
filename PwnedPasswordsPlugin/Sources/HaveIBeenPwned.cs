using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PwnedPasswordsPlugin.Sources
{
    public class HaveIBeenPwned : IPwnedSource
    {
        private readonly HttpClient _client = new HttpClient();

        private const string Url = "https://api.pwnedpasswords.com/range/";

        private const string DescriptionFormat = "This password has been seen {0} times in data breaches and should never be used. If you've ever used it before, change it immediately! " +
            "See https://haveibeenpwned.com/Passwords for more information.";

        private readonly Dictionary<string, Dictionary<string, int>> Cache = new Dictionary<string, Dictionary<string, int>>();

        public async Task<PwInfo> CheckSHA1HashAsync(string hash)
        {
            var prefix = hash.Substring(0, 5);
            var remaining = hash.Substring(5);
            Logger.Log("Initial: {0}, Remaining: {1}", prefix, remaining);

            var cached = GetFromCache(prefix, remaining);
            if (cached != null) return cached;
            
            var resp = await _client.GetAsync(Url + prefix);
            var data = await resp.Content.ReadAsStringAsync();
            FillCache(prefix, data);

            return GetFromCache(prefix, remaining)
                ?? new PwInfo { IsPwned = false };
        }

        private PwInfo GetFromCache(string initial, string remaining)
        {
            Dictionary<string, int> cacheSub;
            if (Cache.TryGetValue(initial, out cacheSub))
            {
                int count;
                if (cacheSub.TryGetValue(remaining, out count))
                {
                    return new PwInfo
                    {
                        IsPwned = true,
                        Description = String.Format(DescriptionFormat, count)
                    };
                }
            }
            return null;
        }

        private void FillCache(string prefix, string data)
        {
            var lines = data.Split();
            foreach(var line in lines)
            {
                var items = line.Split(':');
                if(items.Length != 2)
                {
                    continue;
                }
                var hash = items[0];
                var count = Int32.Parse(items[1]);

                Dictionary<string, int> cacheSub;
                if (!Cache.TryGetValue(prefix, out cacheSub))
                {
                    Cache[prefix] = cacheSub = new Dictionary<string, int>();
                }

                cacheSub[hash] = count;
            }
        }
    }
}
