using KeePassLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PwnedPasswordsPlugin
{
    public class PasswordChecker
    {
        private readonly List<Type> SourceTypes = new List<Type>();

        private static readonly DateTime ExpiredDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly TimeSpan LastCheckedDuration = TimeSpan.FromDays(1);

        private const string MarkedPwnedAtKey = "PwnedPasswords.MarkedPwnedAt";

        private const string LastCheckDateKey = "PwnedPasswords.LastChecked";

        private const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public void RegisterSource<T>() where T : IPwnedSource, new()
        {
            SourceTypes.Add(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="prefs"></param>
        /// <param name="forceRecheck">Force all passwords to be rechecked, regardless of pwned status or check date</param>
        /// <returns></returns>
        public async Task<bool> CheckPasswordStatusRunner(IEnumerable<PwEntry> entries, PreferencesAdapter prefs, bool forceRecheck)
        {
            var anyChange = false;
            try
            {
                var sources = new List<IPwnedSource>();
                foreach (var typ in SourceTypes)
                {
                    sources.Add((IPwnedSource)Activator.CreateInstance(typ));
                }

                foreach (var entry in entries)
                {
                    Logger.Log("Checking entry {0}", entry.Strings.ReadSafe("Title"));

                    if (!forceRecheck)
                    {
                        var lastChecked = entry.CustomData.Get(LastCheckDateKey);
                        if (!String.IsNullOrEmpty(lastChecked))
                        {
                            var checkedDate = DateTime.ParseExact(lastChecked, DateFormat, CultureInfo.InvariantCulture);
                            var unow = DateTime.UtcNow;
                            if (checkedDate + LastCheckedDuration > unow)
                            {
                                Logger.Log("Was already checked recently. Ignoring.", checkedDate, unow);
                                continue;  // Don't recheck yet
                            }
                        }

                        var pwnedAt = entry.CustomData.Get(MarkedPwnedAtKey);
                        if (!String.IsNullOrEmpty(pwnedAt))
                        {
                            var pwnedDate = DateTime.ParseExact(pwnedAt, DateFormat, CultureInfo.InvariantCulture);
                            if (pwnedDate < entry.LastModificationTime)
                            {
                                Logger.Log("removing pwned marker");
                                entry.CustomData.Remove(MarkedPwnedAtKey);  // remove pwn marker if password has been modified
                            }
                            else
                            {
                                Logger.Log("Marked as pwned already, ignoring");
                                continue;  // already marked as pwned, don't check again
                            }
                        }
                    }

                    var pw = entry.Strings.ReadSafe("Password");
                    Logger.Log("Password is {0}", pw);
                    if (String.IsNullOrEmpty(pw))
                    {
                        continue;
                    }

                    var sha = SHA1.Create();
                    var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pw));
                    var hash = HexStringFromBytes(hashBytes);

                    Logger.Log("SHA hash is {0}", hash);

                    var now = DateTime.UtcNow.ToString(DateFormat, CultureInfo.InvariantCulture);
                    foreach (var source in sources)
                    {
                        var result = await source.CheckSHA1HashAsync(hash);
                        if (result.IsPwned)
                        {
                            anyChange = true;
                            Logger.Log("Password is pwned");
                            if (prefs.ExpireEntryOnPwn)
                            {
                                entry.Expires = true;
                                entry.ExpiryTime = ExpiredDateTime;
                            }
                            if (prefs.AddNoteOnPwn && result.Description != null)
                            {
                                var notesP = entry.Strings.GetSafe("Notes");
                                var notes = notesP.ReadString();
                                if (!String.IsNullOrWhiteSpace(notes))
                                {
                                    notes += "\r\n\r\n";
                                }
                                notes += result.Description;
                                Logger.Log("Setting new description:\r\n{0}", notes);
                                entry.Strings.Set("Notes", new KeePassLib.Security.ProtectedString(notesP.IsProtected, notes));
                            }
                            entry.CustomData.Set(MarkedPwnedAtKey, now);
                            break;
                        }
                    }

                    entry.CustomData.Set(LastCheckDateKey, now);
                };
            }
            catch(Exception e)
            {
                Logger.Log(e.ToString());
            }

            return anyChange;
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("X2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
