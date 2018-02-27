using System;
using System.Windows.Forms;

namespace PwnedPasswordsPlugin
{
    public partial class PreferencesForm : Form
    {
        private readonly Action CheckAllCallback;

        public PreferencesForm(Action checkAllCallback)
        {
            InitializeComponent();
            AcceptButton = saveButton;
            if (checkAllCallback != null)
            {
                CheckAllCallback = checkAllCallback;
            }
            else
            {
                checkAllNowButton.Enabled = false;
            }
            
        }

        public bool ExpirePasswordEntryOnPwn
        {
            get { return expireOnPwn.Checked; }
            set { expireOnPwn.Checked = value; }
        }

        public bool AddNoteOnPwn
        {
            get { return addNoteOnPwn.Checked; }
            set { addNoteOnPwn.Checked = value; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkAllNowButton_Click(object sender, EventArgs e)
        {
            CheckAllCallback();
            checkAllNowButton.Enabled = false;
        }
    }
}
