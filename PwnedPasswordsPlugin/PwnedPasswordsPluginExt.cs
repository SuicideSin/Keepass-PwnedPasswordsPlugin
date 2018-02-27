using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePass.Ecas;
using KeePass.Plugins;
using KeePassLib;

namespace PwnedPasswordsPlugin
{
    public sealed class PwnedPasswordsPluginExt : Plugin
    {
        private IPluginHost _host;
        private readonly PasswordChecker Checker;
        private readonly Image Icon;

        private ToolStripSeparator _sep;
        private ToolStripMenuItem _item;

        private PreferencesAdapter _prefs;

        private Task _backgroundChecker;

        private const string DatabaseOpenedTriggerUuid = "5f8TBoW4QYm5BvaeKztApw==";

        public PwnedPasswordsPluginExt()
        {
            Checker = new PasswordChecker();
            Checker.RegisterSource<Sources.HaveIBeenPwned>();

            var strm = typeof(PwnedPasswordsPluginExt).Assembly
                .GetManifestResourceStream("PwnedPasswordsPlugin.icon.ico");
            Icon = Image.FromStream(strm);
        }

        public override Image SmallIcon => Icon;
        

        public override bool Initialize(IPluginHost host)
        {
            _host = host;

            // workaround to support Tsl1.2
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            _prefs = new PreferencesAdapter(host.CustomConfig);
            SetupMenu();

            host.TriggerSystem.RaisingEvent += TriggerSystem_RaisingEvent;

            return true;
        }

        private void TriggerSystem_RaisingEvent(object sender, EcasRaisingEventArgs e)
        {
            if(e.Event.TypeString == DatabaseOpenedTriggerUuid)
            {
                RunTaskInBackground(false);
            }
        }

        public void RunTaskInBackground(bool forceRecheck)
        {
            _backgroundChecker = Task.Factory.StartNew(async () =>
            {
                _host.Database.Modified = await Checker.CheckPasswordStatusRunner(
                    _host.Database.RootGroup.GetEntries(true),
                    _prefs,
                    forceRecheck);
            });
        }

        private void SetupMenu()
        {
            var menu = _host.MainWindow.ToolsMenu.DropDownItems;

            _sep = new ToolStripSeparator();
            menu.Add(_sep);

            _item = new ToolStripMenuItem();
            _item.Image = SmallIcon;
            _item.Text = "Pwned Passwords Preferences";
            _item.Click += OnPreferencesOpened;
            menu.Add(_item);
        }

        private void OnPreferencesOpened(object sender, EventArgs e)
        {
            PreferencesForm form;
            if (_host.Database.IsOpen && (_backgroundChecker == null || _backgroundChecker.IsCompleted))
            {
                form = new PreferencesForm(() => RunTaskInBackground(true));
            }
            else
            {
                form = new PreferencesForm(null);
            }
            form.ExpirePasswordEntryOnPwn = _prefs.ExpireEntryOnPwn;
            form.AddNoteOnPwn = _prefs.AddNoteOnPwn;

            var result = form.ShowDialog();
            if(result == DialogResult.OK)
            {
                _prefs.ExpireEntryOnPwn = form.ExpirePasswordEntryOnPwn;
                _prefs.AddNoteOnPwn = form.AddNoteOnPwn;
            }
        }

        public override string UpdateUrl => "https://raw.githubusercontent.com/nemec/Keepass-PwnedPasswordsPlugin/master/keepass.version";

        public override void Terminate()
        {
            var menu = _host.MainWindow.ToolsMenu.DropDownItems;
            _item.Click -= OnPreferencesOpened;
            menu.Remove(_item);
            menu.Remove(_sep);
        }
    }
}
