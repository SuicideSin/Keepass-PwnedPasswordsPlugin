using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnedPasswordsPlugin
{
    public static class Logger
    {
        private const string Path = @"C:\Users\dan\prg\PwnedPasswordsPlugin\KeePass-2.38\log.txt";

        public static void Log(string message)
        {
            File.AppendAllText(Path, String.Format("{0}: {1}\r\n", DateTime.UtcNow, message));
        }

        public static void Log(string message, params object[] values)
        {
            File.AppendAllText(Path, String.Format("{0}: {1}\r\n", DateTime.UtcNow, String.Format(message, values)));
        }
    }
}
