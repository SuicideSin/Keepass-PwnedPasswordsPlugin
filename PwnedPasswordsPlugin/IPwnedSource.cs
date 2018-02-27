using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnedPasswordsPlugin
{
    public interface IPwnedSource
    {
        Task<PwInfo> CheckSHA1HashAsync(string hash);
    }
}
