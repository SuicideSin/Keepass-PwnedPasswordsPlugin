using KeePass.App.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwnedPasswordsPlugin
{
    public class PreferencesAdapter
    {
        private const string ConfigNamespace = "PwnedPasswords.{0}";
        private string GetConfigKey(string prop) => String.Format(ConfigNamespace, prop);

        private readonly AceCustomConfig Config;

        public PreferencesAdapter(AceCustomConfig config)
        {
            Config = config;
        }

        public void SetDefaults()
        {
            ExpireEntryOnPwn = true;
            AddNoteOnPwn = true;
        }

        public bool ExpireEntryOnPwn
        {
            get
            {
                return Config.GetBool(GetConfigKey("ExpireOnPwn"), true);
            }
            set
            {
                Config.SetBool(GetConfigKey("ExpireOnPwn"), value);
            }
        }

        public bool AddNoteOnPwn
        {
            get
            {
                return Config.GetBool(GetConfigKey("AddNoteOnPwn"), true);
            }
            set
            {
                Config.SetBool(GetConfigKey("AddNoteOnPwn"), value);
            }
        }
    }
}
